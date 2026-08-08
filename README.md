<div align="center">

# 🌐 Ondyxn

**Um navegador moderno e open-source construído com Avalonia UI e CefGlue**

[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://docs.microsoft.com/pt-br/dotnet/csharp/)
[![Avalonia](https://img.shields.io/badge/Avalonia-11-purple?style=for-the-badge)](https://avaloniaui.net/)
[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![CEF](https://img.shields.io/badge/CEF-Chromium-4285F4?style=for-the-badge)](https://bitbucket.org/chromiumembedded/cef/)
[![License](https://img.shields.io/badge/License-MIT-blue?style=for-the-badge)](LICENSE)

<img src="docs/preview.png" alt="Ondyxn Preview" width="800"/>

*Um navegador leve, rápido e personalizável com interface moderna*

</div>

---

## ✨ Funcionalidades

- 🎨 **Interface Moderna** — Design glassmorphism com cantos arredondados e efeitos de blur
- 🔖 **Sistema de Abas** — Gerencie múltiplas abas com facilidade
- 📑 **Página Nova Aba** — Página inicial personalizada com links rápidos
- 🔍 **Omnibox Inteligente** — Barra de endereços com pesquisa integrada
- 📥 **Gerenciador de Downloads** — Acompanhe seus downloads
- 🔖 **Favoritos** — Salve e organize seus sites preferidos
- 📜 **Histórico** — Navegue pelo seu histórico de visitas
- 🛡️ **Bloqueio de Anúncios** — Proteção integrada contra anúncios
- ⌨️ **Atalhos de Teclado** — Produtividade com atalhos como no Chrome
- 🌙 **Modo Privado** — Navegue sem salvar dados
- 📊 **DevTools** — Ferramentas de desenvolvedor integradas (F12)

## 🚀 Atalhos de Teclado

| Atalho | Ação |
|--------|------|
| `Ctrl+T` | Nova aba |
| `Ctrl+W` | Fechar aba |
| `Ctrl+Tab` | Próxima aba |
| `Ctrl+Shift+Tab` | Aba anterior |
| `Ctrl+L` | Focar na barra de endereços |
| `Ctrl+R` | Recarregar página |
| `Ctrl+J` | Abrir downloads |
| `Ctrl+D` | Adicionar favorito |
| `F5` | Recarregar página |
| `F12` | Abrir DevTools |
| `Alt+←` | Voltar |
| `Alt+→` | Avançar |

## 📋 Pré-requisitos

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) ou superior
- Windows 10/11 (suporte a macOS e Linux em desenvolvimento)

## 🛠️ Como Compilar

```bash
# Clone o repositório
git clone https://github.com/shindozk/Ondyxn.git
cd Ondyxn

# Restaure as dependências
dotnet restore

# Compile o projeto
dotnet build

# Execute o navegador
dotnet run --project src/Ondyxn
```

## 📁 Estrutura do Projeto

```
Ondyxn/
├── src/
│   ├── Ondyxn/                    # Projeto principal (entry point)
│   ├── Ondyxn.Core/               # Modelos, interfaces e enums compartilhados
│   ├── Ondyxn.Data/               # Camada de dados (SQLite + Entity Framework)
│   ├── Ondyxn.Engine/             # Integração CEF (CefGlue) e serviços do navegador
│   ├── Ondyxn.UI/                 # Interface do usuário (Avalonia XAML)
│   └── Ondyxn.Tests/              # Testes unitários
├── docs/                          # Documentação e imagens
├── Ondyxn.sln                     # Solution file
└── README.md
```

### 🏗️ Arquitetura

| Camada | Responsabilidade |
|--------|-----------------|
| **Ondyxn.Core** | Modelos de dados, interfaces e enums |
| **Ondyxn.Data** | Persistência com SQLite via Entity Framework |
| **Ondyxn.Engine** | Integração CefGlue, handlers de rede, navegação |
| **Ondyxn.UI** | Interface Avalonia, ViewModels, controles customizados |

## 🧪 Testes

```bash
# Execute todos os testes
dotnet test

# Execute testes específicos
dotnet test src/Ondyxn.Tests
```

## 🤝 Contribuindo

Contribuições são muito bem-vindas! Siga estes passos:

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/nova-feature`)
3. Commit suas mudanças (`git commit -m 'Adiciona nova feature'`)
4. Push para a branch (`git push origin feature/nova-feature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está licenciado sob a Licença MIT - veja o arquivo [LICENSE](LICENSE) para detalhes.

## 🔗 Links Úteis

- [Avalonia UI](https://avaloniaui.net/) — Framework UI cross-platform
- [CefGlue](https://github.com/nickvdyck/cefglue) — Bindings CEF para .NET
- [CefGlue.Avalonia](https://github.com/nickvdyck/cefglue) — Integração CefGlue com Avalonia

---

<div align="center">

Feito com ❤️ por [shindozk](https://github.com/shindozk)

</div>
