ЭТАП 1: Аватар и Стабильность (Здоровье, Передвижение, Глитчи)
Суть: Здоровье персонажа называется "Стабильность" (Max: 100). При получении урона она падает. Если Стабильность опускается ниже 30% — персонаж "ломается": отключается возможность бега (спринта) и на весь экран (или на модельку) накладывается сильный визуальный глитч.
1.1. Логика Стабильности (StabilitySystem.cs)
Этот скрипт у вас уже почти готов, он висит на игроке и врагах.
Что происходит: При получении урона вызывается метод TakeDamage(Damage). Стабильность пересчитывается.
Связь с другими системами: При изменении значения вызывается EventBus.Raise(new PlayerStabilityChangedEvent...). В этот ивент передается флаг IsGlitched (если Stability / MaxStability < 0.3f).
1.2. Ограничение передвижения (MovementController.cs и State Machine)
Вам нужно связать текущую стейт-машину с уровнем Стабильности игрока, чтобы физически запретить ему бегать при критическом уроне.
Как сделать в коде:
В MovementController.cs в методе Awake (или Init) получаем ссылку на StabilitySystem:
_stabilitySystem = GetComponent<StabilitySystem>();
Добавляем публичное свойство для проверки:
public bool CanSprint => (_stabilitySystem.Stability / _stabilitySystem.MaxStability) > 0.3f;
В методе MoveStatesInit() модифицируем условия переходов для sprintingState:
code
C#
// Переход ИЗ ходьбы В спринт: кнопка нажата И стабильность > 30%
_moveStateMachine.AddTransition(walkingState, sprintingState, 
    () => _inputManager.SprintInput && CanSprint);

// Переход ИЗ спринта В ходьбу: кнопка отпущена ИЛИ стабильность упала < 30%
_moveStateMachine.AddTransition(sprintingState, walkingState, 
    () => !_inputManager.SprintInput || !CanSprint);

// Переход ИЗ idle В спринт
_moveStateMachine.AddTransition(idleState, sprintingState, 
    () => _inputManager.SprintInput && CanSprint);
Результат для игрока: Если во время спринта по игроку прилетает урон и ХП падает ниже 30%, стейт-машина мгновенно выкинет его из SprintingState в WalkingState. Скорость упадет, анимация сменится.
1.3. Визуальное отображение Глитча (PlayerGlitchController.cs и PostProcessing)
У вас уже есть этот скрипт, он слушает PlayerStabilityChangedEvent.
Детализация реализации в Unity:
Поскольку игра от 3-го лица, глитч должен применяться в двух местах: на материал самого персонажа и на Post-Processing (экран).
Создайте Shader Graph (или скачайте готовый) для материала Аватара. В нем должно быть свойство _GlitchIntensity (от 0 до 1). Оно управляет смещением вершин (Vertex Position) и шумом в текстуре (UV distortion).
В PlayerGlitchController мы берем MeshRenderer игрока и передаем туда значение:
code
C#
float intensity = eventData.IsGlitched ? 1f : (1f - eventData.StabilityPercent) * 0.2f;
_playerMaterial.SetFloat("_GlitchIntensity", intensity);
Для Post-Processing (хроматическая аберрация на весь экран) — можно сделать отдельный скрипт ScreenGlitchController, который тоже слушает этот ивент и через VolumeProfile увеличивает Chromatic Aberration и Lens Distortion.
Звук: Если IsGlitched == true, можно через AudioManager запускать фоновый зацикленный звук тихих радиопомех.
Резюме 1-го этапа: Игрок получает урон -> Стабильность падает -> Ивент летит по шине -> MovementController блокирует бег -> PlayerGlitchController ломает модельку и экран.
ЭТАП 2: Сканер, Визор и Дебаггинг
Суть: Сканер — это главный инструмент датамайнера. По нажатию [Tab] обычное зрение заменяется "Визором разработчика" (экран синеет, появляется рамка). В этом режиме игрок начинает видеть скрытые "сломанные" объекты — они подсвечиваются красным аутлайном. Если подойти к такому объекту и нажать [E], игра ставится на паузу (или отключается управление) и открывается UI-головоломка (мини-игра).
2.1. Визуал "Визора" (ScannerView и Post-Processing)
Нам нужно создать визуальное отличие режима сканирования от обычного мира.
UI-составляющая (ScannerView.cs):
Создаете новый класс ScannerView : View (наследуется от вашего базового класса окон).
В Unity создаете Canvas-объект, вешаете на него этот скрипт. Внутри Canvas добавляете Image с полупрозрачной синей рамкой по краям экрана и, например, скан-линией (как в старых ЭЛТ-мониторах). Добавляете этот View в ViewManager.
Связь со скриптом (ScannerController.cs):
В вашем ScannerController добавляем инъекцию ViewManager и меняем логику переключения:
code
C#
[Inject] private ViewManager _viewManager;
private ScannerView _scannerView;

public void Init()
{
    _scannerView = _viewManager.GetView<ScannerView>();
    _inputManager.OnScannerPressed += ToggleScanner;
    _inputManager.OnInteractPressed += TryInteract;
}

private void ToggleScanner()
{
    IsScannerActive = !IsScannerActive;
    
    if (IsScannerActive)
        _scannerView.Show(); // Включаем синюю рамку
    else
    {
        _scannerView.Hide(); // Выключаем рамку
        if (_currentTarget != null)
        {
            _currentTarget.OnScanned(false);
            _currentTarget = null;
        }
    }
}
Совет для "вау-эффекта": Вместе с включением ScannerView можно активировать отдельный профиль Volume (Post-Processing) с Color Grading (в сине-зеленых тонах) и сильной виньеткой.
2.2. Поиск сломанного кода (Логика Raycast)
В вашем ScannerController.cs уже написан метод Update(), который пускает луч из центра экрана. Эта логика отличная, её оставляем.
Что происходит: Каждый кадр луч летит на _scanDistance (например, 5 метров).
Слой _interactableLayer: Обязательно в Unity создайте слой (Layer) "Interactable". Повесьте его на все ящики, консоли и двери, которые можно сканировать. В инспекторе ScannerController выберите эту маску. Это сильно сэкономит ресурсы (Raycast не будет проверять стены и пол).
Аутлайн (Обводка): У вас в BuggablePhysicsBox.cs уже заложена ссылка на компонент Outline. Убедитесь, что вы скачали любой бесплатный скрипт Outline (например, QuickOutline из Asset Store) и повесили на объект. В методе OnScanned(true) обводка включается и красится в красный цвет.
2.3. Переход к мини-игре (Дебаггинг)
Когда игрок смотрит на подсвеченный красным объект и нажимает [E], срабатывает метод OnInteract().
Как это работает сейчас: У вас вызывается FixBug(), и объект чинится мгновенно.
Как должно работать по Диздоку v2:
Объект (например, забагованный ящик BuggablePhysicsBox) должен "захватить" фокус игрока и открыть UI-пазл.
Пример обновленного метода OnInteract в классе BuggablePhysicsBox:
code
C#
[Inject] private ViewManager _viewManager;
[Inject] private InputManager _inputManager;

public void OnInteract()
{
    if (!IsBugged) return;

    // 1. Открываем нужную мини-игру и передаем ей ссылку на этот объект (this),
    // чтобы мини-игра знала, кого "чинить" после победы.
    var puzzleView = _viewManager.GetView<PhysicsPuzzleView>();
    puzzleView.ShowPuzzle(this);

    // 2. Отключаем управление игрока и показываем мышку
    // (Потребуется добавить методы DisablePlayerInput/EnablePlayerInput в InputManager)
    _inputManager.DisablePlayerInput();
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
}
2.4. Починка объекта (FixBug())
Когда игрок успешно решает мини-игру (об этом в следующем этапе), мини-игра вызывает метод FixBug() у того объекта, который ей передали.
В этот момент BuggablePhysicsBox делает следующее:
IsBugged = false;
Убирает красную обводку (или меняет на короткую зеленую вспышку).
Применяет изменения физики: _rb.mass = 10f; _rb.isKinematic = false; (теперь ящик упадет на пол и его можно толкать).
Опционально: можно использовать EventBus.Raise(new ObjectFixedEvent()) и проиграть звук успешной компиляции (радостный писк) через AudioManager.
Резюме 2-го этапа: Игрок нажимает Tab -> Экран синеет -> Луч находит ящик -> Ящик загорается красным -> Игрок жмет E -> Управление отключается, появляется курсор мыши, открывается 2D-интерфейс мини-игры поверх экрана.
ЭТАПА 3: UI-Пазлы (Мини-игры). Это ядро вашего жанра "Immersive Sim / Puzzle", где датамайнер физически "копается" в коде.
Суть: Мини-игры реализованы поверх основного геймплея (как 2D-интерфейс на Canvas). Игровой мир на фоне не ставится на жесткую паузу (Time.timeScale = 1), но управление персонажем отключается, а курсор мыши разблокируется. При успешном решении головоломки вызывается FixBug() у конкретного объекта.
Давайте разберем архитектуру обеих мини-игр максимально подробно.
3.1. Базовая логика для всех пазлов
Чтобы не дублировать код закрытия окна и возврата управления, создадим базовый класс для всех пазлов.
Класс PuzzleViewBase : View
code
C#
using Core.UI;
using Core.Input;
using Core.DI;
using Gameplay.Interactables;
using UnityEngine;

public abstract class PuzzleViewBase : View, IInjectable
{
    [Inject] protected InputManager _inputManager;
    protected IBuggable _currentTarget;

    // Вызывается из сканируемого объекта
    public virtual void ShowPuzzle(IBuggable target)
    {
        _currentTarget = target;
        Show(); // Включаем UI
    }

    // Вызывается при победе или нажатии кнопки "Отмена"/Esc
    public virtual void ClosePuzzle()
    {
        Hide();
        _currentTarget = null;
        
        // Возвращаем управление игроку
        _inputManager.EnablePlayerInput(); // Убедитесь, что создали эти методы в InputManager
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected void OnWin()
    {
        _currentTarget?.FixBug(); // Чиним объект в мире
        ClosePuzzle();            // Закрываем окно
    }
}
3.2. Мини-игра "Синхронизация физики" (Пазл с блоком)
Эта мини-игра открывается, когда мы сканируем тяжелый ящик, висящий в воздухе (в ветке Alpha).
Визуал (Настройка Canvas в Unity):
Полупрозрачный черный фон (чтобы было видно мир позади).
По центру — интерфейс-окно "Terminal: Physics Property Override".
Три горизонтальных ползунка (стандартный UI Slider). Отключаем у них свойство Interactable для фона, оставляем только ползунок.
За ползунками (в иерархии выше) рисуем зеленую полосу (Image) — это "целевая зона". Например, она находится ровно по центру (значения от 0.45 до 0.55).
Кнопка "APPLY PATCH" (изначально неактивна / interactable = false).
Логика (PhysicsPuzzleView.cs):
Наследуемся от PuzzleViewBase.
code
C#
using UnityEngine.UI;
using UnityEngine;

public class PhysicsPuzzleView : PuzzleViewBase
{
    [SerializeField] private Slider[] _sliders;
    [SerializeField] private Button _applyButton;

    private float _targetMin = 0.4f;
    private float _targetMax = 0.6f;

    public override void ShowPuzzle(IBuggable target)
    {
        base.ShowPuzzle(target);
        
        // Рандомизируем ползунки при каждом открытии
        foreach (var slider in _sliders)
        {
            slider.value = Random.Range(0f, 1f);
            slider.onValueChanged.AddListener(CheckWinCondition);
        }
        
        _applyButton.interactable = false;
        _applyButton.onClick.RemoveAllListeners();
        _applyButton.onClick.AddListener(OnWin);
    }

    private void CheckWinCondition(float value)
    {
        bool isWin = true;
        foreach (var slider in _sliders)
        {
            // Если хотя бы один ползунок вне зеленой зоны - кнопка серая
            if (slider.value < _targetMin || slider.value > _targetMax)
            {
                isWin = false;
                break;
            }
        }
        
        // Если все ползунки в зеленой зоне - кнопка активируется
        _applyButton.interactable = isWin;
        
        // (Опционально) Добавьте щелчки через AudioManager при движении слайдера
    }

    public override void ClosePuzzle()
    {
        base.ClosePuzzle();
        foreach (var slider in _sliders) slider.onValueChanged.RemoveAllListeners();
    }
}
3.3. Мини-игра "Пересборка Коммита" (Пазл с дверью)
Используется на финальной "забагованной" двери в серверной. Это классическая головоломка на сортировку массива.
Визуал (Настройка Canvas в Unity):
Окно с заголовком "Fatal Error: Execution Order Corrupted".
Используем UI-компонент VerticalLayoutGroup.
Внутри 4 прямоугольные кнопки. На кнопках написан код:
1: InitializeSystem();
2: LoadAssets();
3: CompileLogic();
4: ExecuteDoorAnimation();
Цель игрока: выстроить их ровно в этом порядке сверху вниз.
Логика перемещения (CommitPuzzleView.cs):
Как сделать перемещение кнопок? Самый простой и элегантный путь — по клику на кнопку опускать её на 1 позицию вниз. Если она в самом низу — переносить на самый верх. Метод transform.SetSiblingIndex(index) меняет положение UI-элемента в VerticalLayoutGroup.
code
C#
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class CommitPuzzleView : PuzzleViewBase
{
    // Кнопки, которые мы будем перемещать (важно: они должны лежать в VerticalLayoutGroup)
    [SerializeField] private Button[] _codeButtons; 
    
    // Правильный порядок кнопок (задается в инспекторе)
    [SerializeField] private Button[] _correctOrder; 

    public override void ShowPuzzle(IBuggable target)
    {
        base.ShowPuzzle(target);
        
        // Перемешиваем кнопки (просто меняем им SiblingIndex случайным образом)
        for (int i = 0; i < 10; i++)
        {
            int rnd = Random.Range(0, _codeButtons.Length);
            MoveButtonDown(_codeButtons[rnd]);
        }

        // Подписываемся на клики
        foreach (var btn in _codeButtons)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => OnButtonClicked(btn));
        }
    }

    private void OnButtonClicked(Button clickedButton)
    {
        MoveButtonDown(clickedButton);
        CheckWinCondition();
        
        // Тут можно добавить звук щелчка клавиатуры
    }

    private void MoveButtonDown(Button btn)
    {
        int currentIndex = btn.transform.GetSiblingIndex();
        int newIndex = currentIndex + 1;
        
        // Если кнопка была последней, перекидываем ее наверх
        if (newIndex >= _codeButtons.Length) newIndex = 0;
        
        btn.transform.SetSiblingIndex(newIndex);
    }

    private void CheckWinCondition()
    {
        // Проверяем, совпадает ли текущий порядок (SiblingIndex) с эталонным
        for (int i = 0; i < _correctOrder.Length; i++)
        {
            if (_correctOrder[i].transform.GetSiblingIndex() != i)
            {
                return; // Порядок неверный, выходим
            }
        }
        
        // Если цикл прошел до конца - игрок собрал пазл!
        OnWin(); // Вызывает FixBug() у двери и закрывает UI
    }
}
Подключение к ViewManager
Не забудьте в вашем Services.cs (где собираются сервисы) или в самом ViewManager убедиться, что эти две новые вьюшки (PhysicsPuzzleView и CommitPuzzleView) добавлены в список _viewsList в Unity Inspector.
Резюме 3-го этапа: У нас появились две полноценные, работающие мини-игры. Физический пазл проверяет внимательность со слайдерами, а пазл коммита — логическое выстраивание строк кода. Как только игрок решает их, вызывается FixBug(), и мир вокруг него преображается (ящик падает, дверь открывается).

ЭТАП 4: Боевая система и Лут (Парализующий луч и Блоки Кода)
Суть: Боевка переходит из плоскости "просто закликай врага" в тактическую. Монтировка (Melee) наносит урон, а дальняя атака (Spell) превращается в Парализующий луч. При попадании луча враг "зависает" (гловит глитч) на 3 секунды, что позволяет игроку безопасно подойти и нанести урон в ближнем бою.
После смерти из врага выпадает физический объект — Блок Кода, который нужен для решения головоломки с Мостом.
4.1. Парализующий луч и Состояние "Зависания" (Glitch Stun)
Нам нужно научить врага реагировать на оглушение. Для этого создадим новый интерфейс и новое состояние в стейт-машине.
Создаем интерфейс IGlitchable.cs:
code
C#
namespace Gameplay.Combat.Interfaces
{
    public interface IGlitchable
    {
        void ApplyGlitchStun(float duration);
    }
}
Реализуем его в EnemyController.cs:
Откройте EnemyController и добавьте интерфейс. Мы заведем флаг и таймер, чтобы стейт-машина знала, когда переходить в стан.
code
C#
public class EnemyController : MonoBehaviour, ICharacterController, IGlitchable
{
    // ... ваш старый код ...

    public bool IsStunned { get; private set; }
    private float _stunEndTime;

    public void ApplyGlitchStun(float duration)
    {
        IsStunned = true;
        _stunEndTime = Time.time + duration;
    }

    // В Update() добавим сброс стана:
    private void Update()
    {
        if (!_playerTransform) return;
        
        if (IsStunned && Time.time >= _stunEndTime)
        {
            IsStunned = false; // Время стана вышло
        }

        ChasingChecker();
        enemyStateMachine.Tick();
    }
}
Создаем стейт GlitchStunState.cs:
В папке Gameplay\Enemies\States\ создайте новый класс:
code
C#
using Gameplay.Enemies;
using UnityEngine.AI;

namespace Gameplay.Enemies.States
{
    public class GlitchStunState : StatesEnemyConst
    {
        public GlitchStunState(EnemyController enemyController, EnemyAnimator animator, NavMeshAgent navMeshAgent) 
            : base(enemyController, animator, navMeshAgent) { }

        public override void Enter()
        {
            NavMeshAgent.isStopped = true; // Останавливаем движение
            EnemyAnimator._animator.speed = 0f; // Ставим анимацию на паузу (эффект зависания)
            
            // Опционально: включить партиклы глитчей на враге или звук
        }

        public override void Exit()
        {
            EnemyAnimator._animator.speed = 1f; // Возвращаем скорость анимации
            if (NavMeshAgent.isOnNavMesh) NavMeshAgent.isStopped = false;
        }
    }
}
Обновляем EnemyStatesInit() в EnemyController:
Зарегистрируйте новый стейт и добавьте переходы с высшим приоритетом (через AddAnyTransition, чтобы стан мог прервать любую атаку):
code
C#
var stunState = new GlitchStunState(this, enemyAnimator, _agent);

// Переход В стан из ЛЮБОГО состояния (если IsStunned == true)
enemyStateMachine.AddAnyTransition(stunState, () => IsStunned);

// Возврат ИЗ стана в Idle (если IsStunned == false)
enemyStateMachine.AddTransition(stunState, idleState, () => !IsStunned);
Модифицируем Projectile.cs:
Теперь снаряд (луч) при попадании должен не только наносить урон, но и станить.
code
C#
private void OnTriggerEnter(Collider other)
{
    IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
    if (damageable == null || damageable == _self) return;

    // Наносим базовый урон
    DoDamage(damageable);

    // Если это враг, накладываем глитч на 3 секунды
    var glitchable = other.gameObject.GetComponent<IGlitchable>();
    glitchable?.ApplyGlitchStun(3f);

    Destroy(gameObject);
}
(В Unity перекрасьте материал снаряда в ярко-синий/голубой цвет, чтобы он выглядел как замораживающий луч).
4.2. Система Лута (Блоки Кода)
Согласно левел-дизайну, нам нужно собрать 3 блока кода для моста.
Создаем ивент CodeBlockCollectedEvent.cs:
code
C#
using Core.Events;
public struct CodeBlockCollectedEvent : IEvent { }
Создаем префаб Лута:
В Unity создайте светящийся куб (Box Collider с галочкой IsTrigger). Повесьте на него скрипт CodeBlockPickup.cs:
code
C#
using Core.Events;
using UnityEngine;

namespace Gameplay.Interactables
{
    public class CodeBlockPickup : MonoBehaviour
    {
        // Опционально добавьте сюда звук подбора
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EventBus.Raise(new CodeBlockCollectedEvent()); // Сообщаем системе!
                Destroy(gameObject);
            }
        }
    }
}
Спавн Лута при смерти врага:
Откройте EnemyController.cs (или BossDeathState) и добавьте ссылку на префаб.
code
C#
[SerializeField] private GameObject _codeBlockPrefab;

// Добавьте этот метод:
public void SpawnLoot()
{
    if (_codeBlockPrefab != null)
    {
        // Спавним куб на месте врага, чуть приподняв над землей
        Instantiate(_codeBlockPrefab, transform.position + Vector3.up, Quaternion.identity);
    }
}
В DeathState.cs в методе Enter() вызовите: EnemyController.SpawnLoot();.
4.3. Инвентарь и HUD
Теперь нужно, чтобы игра запоминала, сколько блоков мы собрали, и выводила это на экран.
Данные игрока (PlayerData.cs):
Добавьте поле: public int CodeBlocks = 0;
Менеджер инвентаря:
Т.к. у вас уже есть PlayerDataInteractor, можно добавить логику прямо туда, но лучше создать класс-наблюдатель, чтобы не засорять ядро:
code
C#
using Core.Bootstrap;
using Core.DI;
using Core.Events;
using Core.Extensions;
using Core.SaveLoad.PlayerSaves;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IInjectable, IInitializable
{
    [Inject] private PlayerDataInteractor _playerData;

    public void Init()
    {
        EventBus.Subscribe<CodeBlockCollectedEvent>(OnBlockCollected).AddTo(gameObject);
    }

    private void OnBlockCollected(CodeBlockCollectedEvent data)
    {
        _playerData.CurrentSave.CodeBlocks++;
        Debug.Log($"Collected! Total blocks: {_playerData.CurrentSave.CodeBlocks}");
        
        // Здесь можно вызвать ивент для обновления UI: EventBus.Raise(new InventoryUpdatedEvent());
    }
}
(Не забудьте добавить InventoryManager на объект Services на сцене и зарегистрировать в GameEntryPoint).
Обновление интерфейса (PlayerInterfaceView.cs):
В вашем PlayerInterfaceView добавьте TextMeshProUGUI для отображения счетчика (например, [ Blocks: 0 ]). Подпишитесь на изменения (по аналогии со StabilityBarView) и обновляйте текст при подборе!
Резюме 4-го этапа: Теперь боевка стала глубокой: игрок стреляет лучом [ПКМ], враг "встает на паузу" на 3 секунды. Игрок подбегает, бьет его монтировкой [ЛКМ]. Из убитого врага выпадает светящийся куб, при подборе которого увеличивается счетчик инвентаря.

ЭТАПА 5: Наручный Терминал и Прыжки по веткам (Branch Hopping)
Суть: У игрока на руке есть девайс. По нажатию специальной кнопки (например, [C] или [Q]) персонаж поднимает руку, и над ней разворачивается голографический UI терминала. Пока терминал открыт, нажатие кнопки [X] инициирует смену мира (вызов ToggleBranch()).
5.1. Настройка Input (Система ввода)
Нам нужно разделить действия. В вашем InputManager.cs (и в Action Map Unity) добавьте/проверьте наличие двух кнопок:
TerminalToggle (открыть/закрыть терминал на руке).
BranchSwitchSubmit (кнопка [X] для подтверждения прыжка, работает только когда терминал открыт).
5.2. Визуал Терминала (TerminalView.cs)
Создаем голографический интерфейс терминала, который будет появляться на экране.
В Unity: Создайте Canvas, привяжите его не к камере, а сделайте его Render Mode: World Space (чтобы он висел прямо в 3D-мире над левой рукой персонажа) или Screen Space - Overlay, но стилизуйте под интерфейс на руке.
В UI: Текст "TERMINAL OS. Current Branch: [MAIN]. Press [X] to jump to [ALPHA]".
Скрипт TerminalView.cs:
code
C#
using Core.UI;
using Gameplay.Events;
using Core.Events;
using TMPro;
using UnityEngine;

public class TerminalView : View
{
    [SerializeField] private TextMeshProUGUI _branchText;

    private void OnEnable()
    {
        // Подписываемся на смену веток, чтобы обновлять текст
        EventBus.Subscribe<BranchSwitchedEvent>(OnBranchSwitched).AddTo(gameObject);
    }

    private void OnBranchSwitched(BranchSwitchedEvent data)
    {
        _branchText.text = $"Current Branch: {data.NewBranch.ToString()}\nPress [X] to jump.";
    }
}
Не забудьте добавить этот View в список ViewManager.
5.3. Логика Наручного Терминала (WristTerminalController.cs)
Теперь мы переписываем ваш контроллер терминала. Он будет управлять анимацией руки, открытием UI и прослушиванием кнопки [X].
code
C#
using System;
using Core.Bootstrap;
using Core.DI;
using Core.Input;
using Core.UI;
using Gameplay.Core;
using UnityEngine;

namespace Gameplay.Controllers.Player
{
    public class WristTerminalController : MonoBehaviour, IInjectable, IInitializable, IDisposable
    {
        [Inject] private InputManager _inputManager;
        [Inject] private BranchManager _branchManager;
        [Inject] private ViewManager _viewManager;

        private bool _isTerminalOpen = false;

        public void Init()
        {
            // Кнопка открытия терминала
            _inputManager.OnBranchTogglePressed += ToggleTerminal; // Пусть это будет [C]
            // Кнопка самого прыжка [X]
            _inputManager.OnInteractPressed += TryJumpBranch; // Если X висит на OnInteractPressed или добавьте новый Action
        }

        private void ToggleTerminal()
        {
            _isTerminalOpen = !_isTerminalOpen;

            if (_isTerminalOpen)
            {
                // TODO: Вызвать анимацию поднятия левой руки в PlayerAnimator
                _viewManager.GetView<TerminalView>().Show();
                // Опционально: замедлить время слегка (Time.timeScale = 0.5f), чтобы дать игроку подумать
            }
            else
            {
                // TODO: Анимация опускания руки
                _viewManager.GetView<TerminalView>().Hide();
                // Time.timeScale = 1f;
            }
        }

        private void TryJumpBranch()
        {
            // Прыгаем ТОЛЬКО если терминал открыт
            if (_isTerminalOpen)
            {
                _branchManager.ToggleBranch();
                
                // Закрываем терминал после успешного прыжка
                ToggleTerminal(); 
            }
        }

        public void Dispose()
        {
            _inputManager.OnBranchTogglePressed -= ToggleTerminal;
            _inputManager.OnInteractPressed -= TryJumpBranch;
        }
    }
}
5.4. Архитектура Сцены и правило "Анти-Застревание"
Поскольку игрок теперь может прыгнуть в другую ветку в любой момент времени, появляется огромный риск левел-дизайна: игрок стоял в пустом месте в Main, нажал [X], прыгнул в Alpha, а там на этом месте стоит бетонный блок. Игрок застрял в текстурах.
Как это решается (Правила Левел-Дизайна для вас):
Якорная геометрия (Anchor Geometry): Все основные стены, полы, потолки и крупные препятствия должны находиться в объекте SHARED (который никогда не отключается).
Объекты в папках ENV_MAIN и ENV_ALPHA — это только детали (декор, враги, интерактивные пульты, забагованные ящики).
Если в Alpha ветке вы ставите большой уникальный объект (которого нет в Main), убедитесь, что в Main на этом месте стоит какое-то ограждение или яма, чтобы игрок не мог физически зайти в эти координаты перед прыжком.
5.5. Визуальный маскировочный эффект
В BranchManager.cs (как мы обсуждали ранее) обязательно должна быть вспышка или глитч-эффект на 0.2 секунды. При "свободном прыжке" резкая смена текстур без вспышки сильно бьет по глазам.
Самый простой способ: Привязать к BranchManager белый Canvas Image на весь экран и делать ему Alpha 0 -> 1 -> 0 в момент вызова SetActive для веток.
Резюме 5-го этапа: Теперь всё в точности по Диздоку v2.1. Игрок бежит по коридору. Видит яму. Жмет кнопку — на руке загорается голограмма терминала (или UI на экране). Жмет [X]. Экран вспыхивает, звук загрузки, и игрок оказывается в той же точке, но в другой реальности, где мост цел (но рядом бродят враги).

ЭТАПУ 6: Левел-Дизайн и Интерактивные объекты.
Это самый важный этап для сборки вашей демо-версии (Vertical Slice). Здесь мы берем созданные ранее механики и превращаем их в конкретные игровые ситуации, описанные в разделе 5 (Playthrough) вашего Диздока v2.
Для реализации нам понадобятся скрипты конкретных объектов на уровне. Давайте напишем их шаг за шагом.
6.1. Этап 2: "Сломанная физика прототипа" (Нажимная плита и Ящик)
Суть: В ветке Main ящика нет, есть только голограмма-подсказка. В ветке Alpha ящик висит в воздухе с mass = 9999. Игрок чинит его (через сканер и мини-игру), ящик падает. Игрок толкает его на плиту, и открывается дверь в Main.
Правило сборки сцены:
Сама Дверь и Нажимная плита должны лежать в объекте SHARED (чтобы они были в обеих ветках).
Сломанный ящик (BuggablePhysicsBox) лежит в папке ENV_ALPHA.
В ENV_MAIN на месте ящика висит просто UI-текст [Ожидание объекта: Box_Heavy.prefab].
Скрипт простой Двери (SimpleDoor.cs):
code
C#
using UnityEngine;

public class SimpleDoor : MonoBehaviour
{
    [SerializeField] private Vector3 _openOffset = new Vector3(0, 3f, 0);
    [SerializeField] private float _speed = 2f;
    
    private Vector3 _closedPos;
    private Vector3 _targetPos;

    private void Awake()
    {
        _closedPos = transform.position;
        _targetPos = _closedPos;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _targetPos, Time.deltaTime * _speed);
    }

    public void Open() => _targetPos = _closedPos + _openOffset;
    public void Close() => _targetPos = _closedPos;
}
Скрипт Нажимной плиты (PressurePlate.cs):
Вешается на триггер-коллайдер в полу.
code
C#
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private SimpleDoor _linkedDoor;

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что на плиту наехал именно ящик
        if (other.GetComponent<Rigidbody>() != null)
        {
            _linkedDoor.Open();
            // TODO: Звук щелчка плиты
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
        {
            _linkedDoor.Close();
        }
    }
}
6.2. Этап 3: "Сборка моста" (Консоль моста и Экономика)
Суть: Огромная пропасть. В Main стоит чистый пульт, который просит 3 блока кода. Игрок переходит в Alpha, убивает врагов, собирает 3 куба, возвращается в Main, нажимает [E] на пульте — и мост собирается.
Правило сборки сцены:
Объект Моста лежит в SHARED, но изначально он выключен (SetActive(false)).
Голографический пульт лежит в ENV_MAIN.
Враги расставлены в ENV_ALPHA.
Скрипт Пульта (BridgeConsole.cs):
Поскольку это не баг, а обычное взаимодействие, мы сделаем простой триггер.
code
C#
using Core.DI;
using Core.Input;
using Core.SaveLoad.PlayerSaves;
using TMPro;
using UnityEngine;

namespace Gameplay.Interactables
{
    public class BridgeConsole : MonoBehaviour, IInjectable
    {
        [Inject] private PlayerDataInteractor _playerData;
        [Inject] private InputManager _inputManager;

        [SerializeField] private GameObject _bridgeObject;
        [SerializeField] private TextMeshProUGUI _consoleText;
        
        private bool _isPlayerNear;
        private bool _isBridgeBuilt;

        private void OnEnable() => _inputManager.OnInteractPressed += TryBuildBridge;
        private void OnDisable() => _inputManager.OnInteractPressed -= TryBuildBridge;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) _isPlayerNear = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player")) _isPlayerNear = false;
        }

        private void TryBuildBridge()
        {
            if (!_isPlayerNear || _isBridgeBuilt) return;

            if (_playerData.CurrentSave.CodeBlocks >= 3)
            {
                // Списываем блоки
                _playerData.CurrentSave.CodeBlocks -= 3;
                
                // Включаем мост
                _bridgeObject.SetActive(true);
                _isBridgeBuilt = true;
                
                // Обновляем текст
                _consoleText.text = "[Мост собран. Ошибок нет.]";
                _consoleText.color = Color.green;
                
                // TODO: Звук успешной сборки и партиклы!
            }
            else
            {
                Debug.Log("Недостаточно Блоков Кода!");
                // Мигаем красным текстом
            }
        }
    }
}
6.3. Этап 4: "Дебаг логики контроллера" (Мастер-Коммит)
Суть: В Main консоль горит красным, дверь наглухо закрыта. В Alpha консоль искрит, а дверь бешено хлопает. Игрок сканирует искрящую консоль в Alpha, решает мини-игру "Пересборка Коммита". Дверь успокаивается. Игрок возвращается в Main — консоль зеленая, дверь открыта.
Правило сборки сцены:
Дверь лежит в SHARED.
Создаем скрипт-обертку, который реализует интерфейс IBuggable и вешаем его на консоль в ветке Alpha.
Скрипт Забагованной Консоли (DoorCommitConsole.cs):
code
C#
using Core.DI;
using Core.UI;
using Core.Input;
using UnityEngine;

namespace Gameplay.Interactables
{
    public class DoorCommitConsole : MonoBehaviour, IBuggable, IInjectable
    {
        public bool IsBugged { get; private set; } = true;

        [Inject] private ViewManager _viewManager;
        [Inject] private InputManager _inputManager;

        [Header("Ссылки на объекты")]
        [SerializeField] private Animator _doorAnimator; // Аниматор двери в SHARED
        [SerializeField] private MeshRenderer _mainConsoleScreen; // Экран консоли в Main
        [SerializeField] private Material _greenScreenMaterial; // Материал починенной консоли
        [SerializeField] private Outline _outline;

        private void Awake()
        {
            if (_outline != null) _outline.enabled = false;
            
            // Изначально дверь бешено хлопает (анимация с множителем скорости x10)
            _doorAnimator.speed = 10f; 
        }

        public void OnScanned(bool isScanning)
        {
            if (IsBugged && _outline != null)
                _outline.enabled = isScanning;
        }

        public void OnInteract()
        {
            if (!IsBugged) return;

            // Открываем мини-игру с перестановкой кнопок
            // puzzleView.ShowPuzzle(this); (как мы делали в Этапе 3)
            // И отключаем инпут
        }

        public void FixBug()
        {
            IsBugged = false;
            if (_outline != null) _outline.enabled = false;

            // 1. Успокаиваем дверь в Alpha (аниматор скорости возвращается в 0 или 1)
            _doorAnimator.speed = 1f;
            _doorAnimator.Play("Door_Open"); // Проигрываем нормальную анимацию открытия

            // 2. В ветке Main делаем экран консоли зеленым
            if (_mainConsoleScreen != null)
                _mainConsoleScreen.material = _greenScreenMaterial;

            Debug.Log("Логика контроллера восстановлена! Демо пройдено!");
        }
    }
}
Резюме 6-го этапа: Теперь у вас есть вся логическая обвязка (скрипты) для сборки уровня. Вы можете расставить триггеры, накинуть скрипт на плиту, пульт моста и финальную дверь. Архитектура SHARED vs ENV_MAIN / ENV_ALPHA гарантирует, что прыжки во времени не сломают игру, а починенная в одном мире дверь останется открытой в другом!

 ЭТАПОМ 7: Визуал, Шейдеры, Звук и Оптимизация.
Диздок v2 задает очень сочную стилистику: смесь Zelda: BotW (сел-шейдинг, природа) и Half-Life (индустриальность, глитчи). Чтобы ваша дипломная работа выглядела как дорогой инди-проект, а не просто набор кубиков, нужно грамотно настроить "косметику" в Unity.
7.1. Визуальный стиль и Шейдеры (Shader Graph)
Главная фишка вашей картинки — "цифровой распад" (глитчи).
Глитч-Шейдер Аватара (PlayerGlitchController):
Как создать в Unity: Откройте Shader Graph (URP/HDRP). Создайте материал.
Логика нодов: Возьмите ноду Time, умножьте на Noise (Шум). Добавьте этот шум к Vertex Position (позиции вершин).
Свойство _GlitchIntensity: Выведите это значение как открытое (Public) свойство шейдера. Когда оно = 0, моделька выглядит нормально. Когда оно = 1, вершины начинает бешено трясти в разные стороны.
Связь с кодом: Ваш скрипт PlayerGlitchController уже написан так, чтобы менять _GlitchIntensity в зависимости от ХП. Просто накиньте этот материал на персонажа.
Материалы Врагов ("Missing Texture"):
В Диздоке гениальная идея: вместо лиц у врагов текстуры Error 404 или розовая шахматная доска.
Как сделать: Создайте квадратную текстуру в Photoshop (ярко-розовый и черный квадраты, или просто текст "ERROR"). Создайте материал без сглаживания (Filter Mode: Point) и накиньте его на головы врагов. Это мгновенно даст вайб "сломанной игры" без необходимости сложного 3D-моделирования!
Стиль Zelda (Cel-Shading):
Чтобы игра не выглядела слишком реалистично и "тяжело", настройте освещение. Используйте жесткие тени (Hard Shadows). На объекты окружения накиньте простые текстуры с заливкой градиентом (градиентное текстурирование).
7.2. Постобработка (Post-Processing Volumes)
Миры Main и Alpha должны читаться с первой секунды. В Unity это делается через компонент Volume (Global).
Ветка MAIN (Стабильная):
Добавьте Volume. Настройте: Bloom (легкое свечение ламп), Color Grading (сделайте картинку чуть более синеватой/стерильной), Vignette (слабая).
Ветка ALPHA (Сломанная):
Добавьте второй Volume (привяжите его включение/выключение к BranchManager).
Настройте: Film Grain (сильное зерно), Chromatic Aberration (хроматическая аберрация — раздвоение цветов по краям экрана), Color Grading (уведите тени в грязно-зеленый или желтый).
Режим СКАНЕРА (ScannerView):
Создайте отдельный Volume с самым высоким приоритетом (Priority = 10). Изначально выключен (Weight = 0).
В ScannerController при нажатии [Tab] делайте Weight = 1.
В этом профиле выкрутите Color Adjustments так, чтобы всё стало глубоко синим, и добавьте жесткую Vignette, имитируя взгляд через визор шлема.
7.3. Аудио-дизайн (Звук)
Ваш AudioManager отлично написан (с пулом источников и использованием AudioMixer). Докрутим его под атмосферу:
Гул Серверов (Амбиенс):
В AudioMixer создайте группу Ambience.
Для Main ветки: тихий, ровный гул кулеров и кондиционеров.
Для Alpha ветки: тот же гул, но с добавлением электрического треска и искажений.
При вызове BranchManager.SwitchBranch делайте плавный Crossfade (затухание) между этими двумя треками.
Audio Stutter (Глитч звука при уроне):
В Диздоке сказано: Звуки получения урона сопровождаются цифровым треском.
В StabilitySystem в методе TakeDamage вызывайте не просто звук удара (как в фэнтези "Ух!"), а звук "Windows Error" или короткий глитч-эффект.
(Крутая фишка): В вашем VolumeSettings уже есть метод SetSFXLowPass(600) для паузы. Вы можете вызывать его на 0.2 секунды при получении урона — звук будет "глохнуть" (как будто контузия), что подчеркнет потерю "стабильности".
Интерфейс и Сканер:
В ScannerController добавьте _audioManager.PlayUI(...). Звук включения сканера должен быть похож на щелчок фотоаппарата или фокус линзы.
В UI-пазлах (при перетаскивании ползунков) добавьте тихие механические "клики клавиатуры". Это даст мощный иммерсивный эффект "работы с кодом".