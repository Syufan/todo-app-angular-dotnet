# todo-app-angular-dotnet

This is a full-stack TODO list application built with Angular 20 and .NET 9 Web API.


## Features

This application implements the core features of a basic TODO list system:

- View all TODO items (loaded by default)
- Add a new TODO item via form input
- Delete an existing TODO item


## Stack & Versions

- **Frontend**: Angular 20 (May 2025 release)
- **Backend**: ASP.NET Core Web API (.NET 9 STS)

> .NET 9 is the latest release, offering the newest features.  
> For longer-term support, .NET 8 is currently the LTS version.


## Data Model

```mermaid
classDiagram
  class TodoItem {
    int Id
    string Title
  }

## Prerequisites
- Node.js: **20.17.0** （见 `.nvmrc`）
- .NET SDK: **9.0.200**（见 `global.json`）
- （可选）nvm / nvm-windows 用于统一 Node 版本

## Quick Start
```bash
# clone
git clone https://github.com/Syufan/todo-app-angular-dotnet.git
cd todo-app-angular-dotnet

# 使用项目内版本
nvm use || true

# 一键初始化（无数据库）
chmod +x scripts/bootstrap.sh
./scripts/bootstrap.sh


## How to Run

Run Frontend (Angular)
cd client
npm start

Run Backend (.NET)
cd server
dotnet run


Tests & Coverage
	•	Frontend: cd client && npm test -- --watch=false --browsers=ChromeHeadless --code-coverage
	•	Backend: dotnet test ./Server.Tests/Server.Tests.csproj --collect "XPlat Code Coverage"
	•	CI: GitHub Actions 已配置；前后端覆盖率上报到 Codecov（见 Workflow）。


Pre-commit Hooks
	•	前端：Prettier 检查（不自动修复）
	•	后端：dotnet format --verify-no-changes
	•	触发：git commit 时自动运行
	•	本地手动修复：
	•	Frontend: npm --prefix client exec prettier -- --write <file>
	•	Backend: dotnet format（在 server/）

Conventions
	•	行尾：LF（见 .gitattributes）
	•	代码风格：见 .editorconfig
	•	分支策略 / PR 需通过：CI + pre-commit
  # 一次性提交命令
```bash
mkdir -p scripts
# 写入上述文件内容后：
git add .nvmrc global.json .editorconfig .gitattributes scripts/bootstrap.sh README.md
git commit -m "chore: lock dev env & add bootstrap + README"
git push origin main