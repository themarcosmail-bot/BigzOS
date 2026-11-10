# Estado atual — segunda revisão do Claude Code

## Escopo desta revisão

B11, M1/M2, M4, item 9 e M6 da primeira auditoria, mais a decisão de licenciamento GPL-3.0-only.
Nenhuma funcionalidade, pacote ou mudança no código do produto foi adicionada.

- B11: .gitattributes define LF para texto, CRLF para .bat/.cmd e binários preservados.
  Normalização física dos arquivos versionáveis sem staging ou mudança global do Git.
- M1/M2: teste de falha parcial captura stdout e stderr; teste específico de
  falha de memória verifica indisponibilidade, preservação dos outros campos,
  continuidade da saída e retorno 1, sem detalhes internos.
- M4: TESTINGPLATFORM_TELEMETRY_OPTOUT=1 no ambiente local de validação,
  instruções do README e ambiente do CI. Escopo: Microsoft.Testing.Platform,
  quando aplicável; não substitui DOTNET_CLI_TELEMETRY_OPTOUT=1 para a CLI do SDK.
- M6 (na correção inicial): checkout oficial actions/checkout fixado no SHA
  11d5960a326750d5838078e36cf38b85af677262, conferido no ref v4 oficial.
  Permissões globais vazias; somente contents: read no job.
  Token temporário padrão, sem persistência de credenciais ou secrets personalizados.
  Removida a exigência indevida de repositório público. Naquela etapa, CI ainda não executado no GitHub.

## Item 9 — evidências sobre o certificado

Na sessão inicial, a saída de dotnet new mstest com SDK 10.0.403 incluiu:
“Instalou um certificado de desenvolvimento ASP.NET Core HTTPS.”
A saída também informou que a confiança exigiria dotnet dev-certs https --trust.
Essa mensagem é evidência de uma declaração do SDK, não comprovação independente
de instalação, confiança ou remoção de certificado.

O comando inicial definiu DOTNET_CLI_HOME dentro do repositório,
DOTNET_CLI_TELEMETRY_OPTOUT=1 e DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1.
DOTNET_GENERATE_ASPNET_CERTIFICATE=false ainda não estava definido naquele comando.
As execuções posteriores passaram a definir essa variável, além de
DOTNET_ADD_GLOBAL_TOOLS_TO_PATH=false. Isso registra a configuração usada,
sem provar retroativamente o estado do armazenamento.

Não houve inspeção do armazenamento de certificados, execução de dotnet dev-certs,
solicitação de confiança ou tentativa de remoção. Nesta revisão, o armazenamento
não foi acessado nem alterado. O estado efetivo do certificado permanece não verificado.

## Validação das correções anteriores (antes do licenciamento)

Ambiente: Windows x64, SDK .NET 10.0.403. Variáveis do README configuradas no
processo, incluindo os dois opt-outs de telemetria e geração de certificado desativada.

- dotnet build -c Release -p:RestoreConfigFile=NuGet.Config: aprovado,
  zero avisos e zero erros. A propriedade mantém o restore na configuração local.
- dotnet test BigzOS.slnx -c Release --no-build --no-restore: nove testes aprovados,
  zero falhas e zero ignorados. Nenhuma dependência nova foi adicionada.
- Busca por padrões de credenciais em todos os 23 arquivos versionáveis: nenhuma
  ocorrência (tokens conhecidos, chaves privadas, credenciais em URLs e atribuições
  suspeitas). Revisão dos arquivos alterados sem credenciais reais; a string secret
  nos testes é um marcador artificial. Busca por padrões não é prova absoluta.
- Todos os 23 arquivos versionáveis usam LF, sem CR isolado ou CRLF. git check-attr
  confirmou LF para C#/YAML/Markdown e CRLF para futuros scripts .cmd/.bat.
- Comparação com cópia local anterior às correções: nove arquivos com mudanças de
  conteúdo (incluindo .gitattributes novo) e 14 somente com normalização de linhas.
  Código do produto sem mudanças lógicas. Índice Git vazio; não houve git add.
- Diff conferido contra essa cópia anterior, além de git diff --check. Não há
  alterações fora do escopo da auditoria. Naquela etapa, workflow ainda dependia de validação no GitHub.

Arquivos com conteúdo alterado/criado:
.gitattributes; .github/workflows/ci.yml; AGENTS.md; CLAUDE.md; README.md;
docs/DECISIONS.md; docs/SECURITY.md; tasks/CURRENT.md; tests/BigzOS.Tests/Test1.cs.

Somente normalização:
.gitignore; BigzOS.slnx; Directory.Build.props; NuGet.Config; docs/ARCHITECTURE.md;
global.json; src/BigzOS.Cli/BigzOS.Cli.csproj; src/BigzOS.Cli/CliApplication.cs;
src/BigzOS.Cli/Program.cs; src/BigzOS.Core/BigzOS.Core.csproj;
src/BigzOS.Core/SystemInformation.cs; src/BigzOS.Core/WindowsInformationSource.cs;
tests/BigzOS.Tests/BigzOS.Tests.csproj; tests/BigzOS.Tests/MSTestSettings.cs.

## Revisão e pendências

Leia CLAUDE.md e AGENTS.md. Revise especialmente os pontos da auditoria acima.
Licença escolhida pelo mantenedor: GPL-3.0-only. CORE e STUDIO não fazem
parte desta revisão. Não houve commit, push ou alterações de configurações do Windows.

## Licenciamento adicionado para revisão final

- LICENSE: texto oficial integral da GNU, baixado sem edição; 35.149 bytes,
  SHA-256 3972dc9744f6499f0f9b2dbf76696f2ae7ad8af9b23dde66d6af86c9dfb36986.
- README e decisões atualizados para GPL-3.0-only, sem restrições adicionais.
- Directory.Build.props: PackageLicenseExpression e metadado de assembly SPDX;
  inclusão de LICENSE nos futuros pacotes. Testes marcados IsPackable=false.
- docs/LICENSING.md registra o grafo resolvido: 19 pacotes MIT e uma extensão de
  cobertura sob licença própria Microsoft. Sem pacotes NuGet no Core ou CLI.
- Ressalva: a extensão é ferramenta de testes, não aprovada para incorporação ou
  redistribuição como parte de uma obra GPL. Não se declara compatibilidade global
  de todos os artefatos de teste. Nenhuma dependência foi adicionada ou substituída.
- Windows, .NET e componentes Microsoft/terceiros mantêm seus próprios termos.
- Arquivos deste acréscimo: LICENSE, README.md, Directory.Build.props,
  tests/BigzOS.Tests/BigzOS.Tests.csproj, docs/DECISIONS.md, docs/LICENSING.md,
  tasks/CURRENT.md. Demais correções preservadas; sem commit ou push.
- Validação após metadados de licença: build Release com zero erros/avisos;
  suíte completa com nove aprovados, zero falhas e zero ignorados.

## Manutenção do CI — Node.js 24

- Primeira execução bem-sucedida do Windows CI confirmada na API e nos logs:
  [run nº 1, tentativa 3](https://github.com/themarcosmail-bot/BigzOS/actions/runs/42672643435/attempts/3),
  commit 77c17aafc1653c8399714cb263e893ede0e99b57, em 2026-11-09.
  Duração total de 46 segundos (18:56:10–18:56:56 UTC); job build-test de
  41 segundos (18:56:15–18:56:56 UTC).
- [Logs do job](https://github.com/themarcosmail-bot/BigzOS/actions/runs/42672643435/job/129366324363)
  confirmam build com zero avisos/erros e nove testes aprovados, zero falhas e
  zero ignorados. Resultado de testes conferido na saída, não inferido do status.
- Os mesmos logs registram a depreciação do Node.js 20 no checkout anterior:
  actions/checkout@11d5960a326750d5838078e36cf38b85af677262 (v4).
- Pin atualizado para actions/checkout v7.0.1, release oficial estável
  (draft=false, prerelease=false), SHA completo
  3d3c42e5aac5ba805825da76410c181273ba90b1. Procedência verificada antes da edição:
  [release oficial](https://github.com/actions/checkout/releases/tag/v7.0.1),
  [ref da tag](https://api.github.com/repos/actions/checkout/git/ref/tags/v7.0.1) e
  [action.yml no SHA](https://github.com/actions/checkout/blob/3d3c42e5aac5ba805825da76410c181273ba90b1/action.yml),
  que declara runs.using: node24. O README oficial documenta mínimo de runner
  2.327.1 para Node.js 24; o runner do CI observado foi 2.338.2.
- Preservados permissions globais vazias, contents: read no job,
  persist-credentials: false, clean: false, set-safe-directory: false, opt-outs,
  runner Windows, timeout e comandos de restore/build/test. Sem novos triggers,
  secrets, elevação ou funcionalidades. Nenhuma dependência baixada localmente.
- O patch recebido informa validação remota do novo pin no run nº 2, commit
  `a4c8098`. Essa informação não foi verificada nesta execução local; não foi
  iniciado CI nem consultada a rede para confirmar logs remotos.
- Verificações locais desta manutenção: build Release --no-restore com zero
  avisos/erros; suíte completa --no-build --no-restore com nove aprovados,
  zero falhas e zero ignorados. Diff sem erros de whitespace e arquivos em LF.
  Alterações limitadas a .github/workflows/ci.yml e tasks/CURRENT.md.
  Nenhum commit, push ou alteração de configuração do Windows foi executado.

## Bigz Toolkit v0.2.0 — validação local em 2026-11-10

- Checkout exclusivo: C:\Dev\.maestri\floors\bigzos--pilotv020, branch
  pilot/v0.2.0, HEAD a4c8098. Estado inicial: somente ?? .maestri/.
- Fase 1: git apply --check do patch em Downloads retornou 0, sem saída.
  Fase 2: primeira aplicação bloqueada pelo sandbox (código 1), sem mudanças
  no checkout. Após autorização para executar fora do sandbox, git apply
  retornou 0, sem saída. Não houve elevação de administrador do Windows.
- SDK 10.0.403. Antes de todo dotnet foram definidas DOTNET_CLI_HOME e
  NUGET_PACKAGES dentro deste checkout; DOTNET_CLI_TELEMETRY_OPTOUT=1,
  TESTINGPLATFORM_TELEMETRY_OPTOUT=1, DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1,
  DOTNET_GENERATE_ASPNET_CERTIFICATE=false e DOTNET_ADD_GLOBAL_TOOLS_TO_PATH=false.
- Primeiro build Release --no-restore: código 1, NETSDK1004 nos três projetos,
  pois project.assets.json não existia. Restore oficial com NuGet.Config:
  código 0, três projetos restaurados, sem adicionar referências de pacotes.
- Após restore, build Release --no-restore: código 0, zero avisos/erros.
  O primeiro dotnet test retornou 0 sem executar testes. A configuração herdada
  não habilitava o runner MSTest. Corrigidos apenas OutputType=Exe e
  EnableMSTestRunner=true no projeto de testes e test.runner em global.json
  para Microsoft.Testing.Platform, usando os pacotes já existentes.
- Após a correção: dotnet build BigzOS.slnx --configuration Release --no-restore:
  código 0, zero avisos/erros. dotnet test BigzOS.slnx --configuration Release
  --no-build --no-restore: código 0, 12 aprovados, zero falhas e zero ignorados.
  Fontes simuladas preservadas. Incluem JSON completo em bytes, JSON parcial
  com processor/totalMemoryBytes null e retorno 1, versão sem coleta e saída
  textual completa/parcial. Não foi provocada falha na máquina real.
- Smoke local do bigz.exe Release: --version produziu somente 0.2.0 (código 0).
  info --json retornou código 0, exatamente windowsVersion, architecture,
  processor e totalMemoryBytes; memória inteira de 16965443584 bytes.
  info retornou código 0, quatro linhas e memória 15.80 GiB. Assertions de
  nomes dos campos, tipo da memória, versão e equivalência textual passaram.
- git diff --check: código 0, sem saída antes do registro final. Validação
  local Windows x64; não foi executada validação Linux nesta sessão.
- O workflow inclui smoke de versão e JSON reais; o workflow alterado ainda
  não foi executado no GitHub nesta sessão. Permanecem três projetos, sem novas
  dependências, telemetria ou rede no produto, alterações do Windows, staging,
  commit ou push. C:\Dev\BigzOS (main) não foi editado.
