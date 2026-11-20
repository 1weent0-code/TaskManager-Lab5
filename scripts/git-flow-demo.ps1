# Запускати з кореня TaskManager після перевірки програми.
# Скрипт демонструє локальний Git Flow і конфлікт у appsettings.json.
# Перед запуском потрібен встановлений Git і налаштовані user.name/user.email.

$ErrorActionPreference = "Stop"

git init
git add .
git commit -m "chore: початкова версія TaskManager"
git branch -M main
git checkout -b develop

git checkout -b feature/domain-layer
git add Domain/
git commit -m "feat(domain): додати сутність та інтерфейс"
New-Item -ItemType Directory -Force -Path "Domain" | Out-Null
"Domain layer" | Set-Content "Domain/README.md"
git add Domain/README.md
git commit -m "docs(domain): описати доменний шар"
git checkout develop
git merge feature/domain-layer

git checkout -b feature/application-layer
git add Application/
git commit -m "feat(application): додати сценарії використання"
git checkout develop
git merge feature/application-layer

git checkout -b feature/infrastructure-layer
git add Infrastructure/
git commit -m "feat(infrastructure): додати JSON репозиторій"
@'
{
  "Storage": "Json",
  "FileName": "tasks.json"
}
'@ | Set-Content "appsettings.json"
git add appsettings.json
git commit -m "feat(config): налаштувати JSON сховище"
git checkout develop
@'
{
  "Storage": "InMemory",
  "FileName": "memory"
}
'@ | Set-Content "appsettings.json"
git add appsettings.json
git commit -m "config: змінити тип сховища"
git checkout feature/infrastructure-layer
git merge develop
# Очікується конфлікт. Залишаємо JSON-конфігурацію:
@'
{
  "Storage": "Json",
  "FileName": "tasks.json"
}
'@ | Set-Content "appsettings.json"
git add appsettings.json
git commit -m "merge: розв'язати конфлікт конфігурації"
git checkout develop
git merge feature/infrastructure-layer

git checkout -b feature/presentation-layer
git add Presentation/
git commit -m "feat(presentation): додати консольний інтерфейс"
git checkout develop
git merge feature/presentation-layer

dotnet build
git checkout main
git merge develop
git tag v1.0.0

Write-Host "Локальний Git Flow завершено. Для GitHub ще потрібно створити remote, push та Pull Request."
