# BigzOS

Fundação de um projeto para personalização e gerenciamento do Windows 11.
As edições **CORE** (minimalista) e **STUDIO** (equilibrada) são planos futuros.
O primeiro produto é o **Bigz Toolkit**, CLI em C# e .NET 10.

## MVP

O comando `info` exibe versão do Windows, arquitetura do sistema operacional,
nome do processador e memória física total em GiB. Cada campo pode aparecer como
`indisponível` quando uma consulta falha. A versão usa a identificação técnica
fornecida pelo Windows, que pode conter `10.0` no Windows 11; não informa a edição comercial.

Sem telemetria, rede em execução, elevação, escrita no registro ou alterações
de serviços, Defender, UAC, firewall ou Windows Update. Execute como usuário comum.

## Desenvolvimento

Requer SDK .NET 10 estável. O arquivo global.json aceita feature bands 10.0 mais recentes.
Não são necessários workloads. Os únicos pacotes diretos são de testes Microsoft (MSTest).

Antes do primeiro comando do SDK, no PowerShell, mantenha os caches no repositório
e evite os efeitos de inicialização do SDK:

```powershell
$env:DOTNET_CLI_HOME = Join-Path (Get-Location) '.dotnet-home'
$env:NUGET_PACKAGES = Join-Path (Get-Location) '.nuget/packages'
$env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
$env:TESTINGPLATFORM_TELEMETRY_OPTOUT = '1'
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
$env:DOTNET_GENERATE_ASPNET_CERTIFICATE = 'false'
$env:DOTNET_ADD_GLOBAL_TOOLS_TO_PATH = 'false'
dotnet restore BigzOS.slnx --configfile NuGet.Config
dotnet build BigzOS.slnx --configuration Release --no-restore
dotnet test BigzOS.slnx --configuration Release --no-build --no-restore
dotnet run --project src/BigzOS.Cli --configuration Release --no-build -- info
```

Restore usa rede para dependências de desenvolvimento; o executável não usa rede.
Os testes usam fontes simuladas e não consultam nem alteram a máquina real.

`TESTINGPLATFORM_TELEMETRY_OPTOUT=1` desabilita a telemetria da
Microsoft.Testing.Platform quando aplicável; não é um controle geral do SDK .NET.
`DOTNET_CLI_TELEMETRY_OPTOUT=1` é configurada separadamente para a CLI do SDK.
Essas variáveis valem para o processo e seus filhos, sem alterar o Windows.
Referência: [telemetria da plataforma de testes](https://learn.microsoft.com/dotnet/core/testing/microsoft-testing-platform-telemetry).

A política de `.gitattributes` usa LF para texto (C#, MSBuild, YAML e documentação),
CRLF para scripts `.bat`/`.cmd` e preserva binários. Não exige alterar a configuração global do Git.

Códigos de saída: 0 para sucesso/ajuda; 1 para coleta parcial ou plataforma
incompatível; 2 para argumentos inválidos. Comando sem argumentos exibe ajuda.

## Organização

- `src/BigzOS.Core`: contratos, coleta com isolamento de falhas e adaptador Windows.
- `src/BigzOS.Cli`: argumentos, apresentação e códigos de saída.
- `tests/BigzOS.Tests`: testes unitários Microsoft MSTest.
- `docs`: arquitetura, segurança e decisões.
- `tasks/CURRENT.md`: estado e roteiro de revisão.

O CI usa runner Windows hospedado pelo GitHub com SDK 10 instalado e a action
oficial `actions/checkout`, fixada por SHA. O token temporário do GitHub tem apenas
`contents: read` no job e não é persistido no Git. O checkout funciona para o próprio
repositório público ou privado. Não há referência a secrets personalizados, actions
de terceiros, elevação ou deploy. Build e testes não recebem o token em argumentos
ou variáveis de ambiente definidos pelo projeto.

## Licença

O código BigzOS é licenciado sob **GNU General Public License v3.0 only**
(`SPDX-License-Identifier: GPL-3.0-only`), exclusivamente a versão 3.
Consulte o texto oficial integral em [LICENSE](LICENSE). Não há restrições próprias
acrescentadas à GPLv3. Distribuições de obras derivadas devem cumprir a GPLv3,
incluindo as obrigações aplicáveis de disponibilização do código-fonte correspondente.

Essa licença não altera nem substitui as licenças do Windows, do .NET, dos
componentes Microsoft ou de terceiros. Esses componentes mantêm seus próprios termos.
A análise das dependências e a ressalva sobre a ferramenta de cobertura de testes
estão em [docs/LICENSING.md](docs/LICENSING.md).

