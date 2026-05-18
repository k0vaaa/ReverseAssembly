#!/usr/bin/env node

const fs = require('fs').promises;
const path = require('path');

async function combineFiles(startPath, outputFile) {
    try {
        // Определяем абсолютные пути относительно папки, из которой запустили скрипт
        const absoluteStartPath = path.resolve(process.cwd(), startPath);
        const absoluteOutputFile = path.resolve(process.cwd(), outputFile);

        // Проверяем, существует ли исходная папка
        try {
            const stat = await fs.stat(absoluteStartPath);
            if (!stat.isDirectory()) {
                throw new Error();
            }
        } catch (err) {
            console.error(`Ошибка: Папка "${startPath}" не найдена в текущей директории.`);
            return;
        }

        const outputStream = await fs.open(absoluteOutputFile, 'w');

        async function processDirectory(currentPath) {
            // Пропускаем ненужные папки
            const baseName = path.basename(currentPath);
            if (baseName === 'node_modules' || baseName === 'storybook-static') {
                return;
            }

            const entries = await fs.readdir(currentPath, { withFileTypes: true });

            for (const entry of entries) {
                const fullPath = path.join(currentPath, entry.name);
                const ext = path.extname(entry.name).toLowerCase();

                if (entry.isDirectory()) {
                    // Рекурсивный обход
                    await processDirectory(fullPath);
                } else if (entry.isFile() && ['.cs'].includes(ext) && !fullPath.includes("package-lock.json")) {
                    try {
                        const content = await fs.readFile(fullPath, 'utf8');
                        // Делаем путь относительным для красоты в итоговом файле
                        const displayPath = path.relative(process.cwd(), fullPath);
                        await outputStream.write(`\n\n=== ${displayPath} ===\n${content}`);
                    } catch (err) {
                        console.error(`Ошибка при чтении файла ${fullPath}: ${err.message}`);
                    }
                }
            }
        }

        await processDirectory(absoluteStartPath);
        await outputStream.close();
        console.log(`Готово! Все файлы собраны в: ${absoluteOutputFile}`);
    } catch (err) {
        console.error(`Ошибка: ${err.message}`);
    }
}

// Получаем аргументы из командной строки
// process.argv[2] — первый аргумент (откуда брать)
// process.argv[3] — второй аргумент (куда сохранять)
const inputArg = process.argv[2] || './Scripts';
const outputArg = process.argv[3] || './scripts.txt';

combineFiles(inputArg, outputArg);