using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Core.Editor
{
    public abstract class AuditorShrinker
    {
        [MenuItem("Tools/Shrink Auditor Report")]
        public static void ShrinkReport()
        {
            string filePath = EditorUtility.OpenFilePanel("Выберите файл отчета Auditor", "", "");
            if (string.IsNullOrEmpty(filePath)) return;

            // 1. БРОНЕБОЙНАЯ ЗАЩИТА ПУТИ (Гарантированно не трогает оригинал)
            string directory = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string outPath = Path.Combine(directory, fileName + "_ShrunkReport.txt");

            // Если по какой-то магии пути совпали, добавляем суффикс
            if (filePath == outPath) 
                outPath += ".txt"; 

            // 2. Читаем весь минифицированный JSON как один текст
            string json = File.ReadAllText(filePath);
            Dictionary<string, int> groupedIssues = new Dictionary<string, int>();

            // 3. Вытаскиваем нужные ключи регулярным выражением (игнорируя скобки и форматирование)
            MatchCollection matches = Regex.Matches(json, @"\""(description|name|path|relativePath|filename)\""\s*:\s*\""([^\""]+)\""");

            string currentDesc = null;
            string currentPath = null;

            foreach (Match m in matches)
            {
                string key = m.Groups[1].Value;
                string val = m.Groups[2].Value;

                if (key == "description" || key == "name")
                {
                    currentDesc = val;
                    currentPath = null; // Сбрасываем путь для новой проблемы
                }
                else if (key == "path" || key == "relativePath" || key == "filename")
                {
                    currentPath = Path.GetFileName(val); // Оставляем только имя файла
                }

                // Как только нашли и описание, и путь — склеиваем их!
                if (currentDesc != null && currentPath != null)
                {
                    string dictKey = $"[{currentDesc}] --> {currentPath}";
                
                    if (!groupedIssues.ContainsKey(dictKey))
                        groupedIssues[dictKey] = 0;

                    groupedIssues[dictKey]++;

                    // Очищаем, чтобы не задублировать
                    currentDesc = null;
                    currentPath = null; 
                }
            }

            // 4. Формируем красивый отчет
            string report = "=== СЖАТЫЙ ОТЧЕТ PROJECT AUDITOR ===\n";
            report += $"Найдено уникальных проблем: {groupedIssues.Count}\n\n";

            var sorted = groupedIssues.OrderByDescending(x => x.Value);

            foreach (var kvp in sorted)
            {
                report += $"{kvp.Value} шт. \t| {kvp.Key}\n";
            }

            if (groupedIssues.Count == 0)
            {
                report += "ОШИБКА: Парсер не смог найти проблемы. Проверьте формат JSON.";
            }

            // 5. Сохраняем в НОВЫЙ файл
            File.WriteAllText(outPath, report);
        
            Debug.Log($"Готово! Отчет сжат до {new FileInfo(outPath).Length / 1024} KB и сохранен по пути: {outPath}");
            EditorUtility.RevealInFinder(outPath);
        }
    }
}