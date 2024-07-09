
# ShortcutService API

## Обзор
ShortcutService - это API ASP.NET Core, предназначенное для управления горячими клавишами. Оно позволяет добавлять новые горячие клавиши и получать горячие клавиши по категориям. API сопровождается модульными и интеграционными тестами для обеспечения надежной функциональности.

## Содержание
- [Начало работы](#начало-работы)
- [Конечные точки](#конечные-точки)
    - [Добавить горячую клавишу](#добавить-горячую-клавишу)
    - [Получить горячие клавиши по категории](#получить-горячие-клавиши-по-категории)
- [Тестирование](#тестирование)
- [Зависимости](#зависимости)

## Начало работы
### Необходимые условия
- .NET Core 8.0 SDK
- ASP.NET Core 8.0

### Установка
1. Клонируйте репозиторий:
    ```bash
    git clone https://github.com/your-repo/ShortcutService.git
    cd ShortcutService
    ```

2. Восстановите зависимости:
    ```bash
    dotnet restore
    ```

3. Постройте проект:
    ```bash
    dotnet build
    ```

4. Запустите приложение:
    ```bash
    dotnet run
    ```

### Запуск тестов
Для запуска тестов используйте следующую команду:
```bash
dotnet test
```

## Эндпоинты
### Добавить сочетание клавиш
**Эндпоинт:** `POST /shortcuts/add`

**Описание:** Добавляет новую горячую клавишу в сервис.

**Тело запроса:**
```json

{
  "binding": "Ctrl + Shift + K",
  "description": "Push current branch to remote repository",
  "action": "git.push"
}
```

**Ответы:**
- `200 OK`: Сочетание клавиш успешно добавлено.
  ```json
  {
    "success": true
  }
  ```
- `400 Bad Request`: Не удалось добавить сочетание клавиш.
  ```json
  {
    "success": false
  }
  ```

### Получить сочетания клавиш по категории
**Эндпоинт:** `GET /shortcuts/category/{categoryName}`

**Описание:** Получает сочетания клавиш по указанной категории.

**Параметры:**
- `categoryName`: Категория сочетаний клавиш, которые нужно получить.

**Ответы:**
- `200 OK`: Возвращает список сочетаний клавиш.
  ```json
  [
    {
      "ActionName": "string",
      "Binding": "string"
    }
  ]
  ```

## Тестирование
Этот проект включает как модульные, так и интеграционные тесты для обеспечения функциональности API.

### ShortcutService.Tests
Эти тесты охватывают основную логику `ShortcutService` и его компонентов.

### ShortcutService.IntegrationTests
Эти тесты проверяют сквозную функциональность конечных точек API, обеспечивая работу интеграции между компонентами.

## Зависимости
- [ASP.NET Core 8.0](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-8.0)
- [Xunit 2.5.3](https://xunit.net/)
- [FluentAssertions 6.12.0](https://fluentassertions.com/)
- [Microsoft.AspNetCore.Mvc.Testing 8.0.6](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Testing/)
---