# Licenciamento e dependências

SPDX-License-Identifier: GPL-3.0-only

## Código BigzOS

O mantenedor escolheu GNU GPL versão 3 exclusivamente, sem a opção de versões
posteriores: GPL-3.0-only. O texto integral oficial está em [LICENSE](../LICENSE),
baixado diretamente de https://www.gnu.org/licenses/gpl-3.0.txt, sem edição.

Evidência do arquivo recebido: 35.149 bytes; SHA-256
`3972dc9744f6499f0f9b2dbf76696f2ae7ad8af9b23dde66d6af86c9dfb36986`.
O apêndice oficial contém um exemplo “or later”; ele foi preservado como parte
do texto da licença e não muda a escolha GPL-3.0-only expressa no README e metadados.
Referência do identificador: [SPDX](https://spdx.org/licenses/GPL-3.0-only.html).

Distribuições de código BigzOS e de obras derivadas devem cumprir a GPLv3,
incluindo os requisitos aplicáveis ao código-fonte correspondente.
Este documento explica a decisão; não acrescenta restrições ou termos à GPLv3.

## Limites

O licenciamento do BigzOS não altera as licenças do Windows, do SDK/runtime .NET,
de componentes Microsoft ou de terceiros. Não concede direitos de redistribuição
desses componentes além dos previstos em suas próprias licenças.
O uso de APIs Windows não torna o Windows parte do código BigzOS.
Não há redistribuição de Windows ou runtime .NET nesta fundação.

## Verificação das dependências restauradas

Inventário extraído de `tests/BigzOS.Tests/obj/project.assets.json` e dos
`.nuspec` dos pacotes exatos restaurados em `.nuget/packages`, em 2026-11-09.
O inventário usa o grafo resolvido, não todas as versões presentes no cache.
Core e CLI não têm dependências de pacotes NuGet: somente referência entre
projetos e APIs do framework. A árvore abaixo pertence ao projeto de testes.

São 20 pacotes: 19 declaram MIT e um declara licença própria por arquivo.
MIT/Expat é compatível com GPL segundo a
[FSF](https://www.gnu.org/licenses/license-list.html#Expat).
A incorporação de componentes MIT deve preservar os avisos de copyright e licença.
Essa compatibilidade não significa relicenciamento automático desses componentes.


| Pacote resolvido | Licença declarada |
| --- | --- |
| Microsoft.ApplicationInsights/2.23.0 | MIT |
| Microsoft.CodeCoverage/18.0.1 | MIT |
| Microsoft.DiaSymReader/2.0.0 | MIT |
| Microsoft.Extensions.DependencyModel/6.0.2 | MIT |
| Microsoft.NET.Test.Sdk/18.0.1 | MIT |
| Microsoft.Testing.Extensions.CodeCoverage/18.1.0 | Microsoft .NET Library (License.txt), não MIT |
| Microsoft.Testing.Extensions.Telemetry/2.0.2 | MIT |
| Microsoft.Testing.Extensions.TrxReport/2.0.2 | MIT |
| Microsoft.Testing.Extensions.TrxReport.Abstractions/2.0.2 | MIT |
| Microsoft.Testing.Extensions.VSTestBridge/2.0.2 | MIT |
| Microsoft.Testing.Platform/2.0.2 | MIT |
| Microsoft.Testing.Platform.MSBuild/2.0.2 | MIT |
| Microsoft.TestPlatform.AdapterUtilities/18.0.1 | MIT |
| Microsoft.TestPlatform.ObjectModel/18.0.1 | MIT |
| Microsoft.TestPlatform.TestHost/18.0.1 | MIT |
| MSTest/4.0.2 | MIT |
| MSTest.Analyzers/4.0.2 | MIT |
| MSTest.TestAdapter/4.0.2 | MIT |
| MSTest.TestFramework/4.0.2 | MIT |
| Newtonsoft.Json/13.0.3 | MIT |

## Ressalva: ferramenta de cobertura

Microsoft.Testing.Extensions.CodeCoverage 18.1.0 entra pelo metapacote MSTest 4.0.2.
Seu License.txt permite uso para desenvolvimento/testes, mas contém restrições de
redistribuição e de submissão do componente a uma “Excluded License”. Portanto,
não foi classificado como compatível para incorporação em uma obra distribuída
sob GPLv3. Isso não impede licenciar o código próprio BigzOS sob GPL-3.0-only.

O uso atual é de ferramenta de desenvolvimento/testes sob os próprios termos
Microsoft; não integra a CLI ou Core. O projeto de testes tem IsPackable=false,
mas isso não impede cópia manual de seus binários. A distribuição do código-fonte
BigzOS não inclui o cache NuGet, ignorado pelo Git.

Não há aprovação para redistribuir a extensão ou os artefatos de testes como parte
de um produto GPL. Antes de distribuir qualquer pacote com ferramentas de testes,
será necessário remover/substituir essa extensão ou analisar separadamente a forma
de distribuição e os termos aplicáveis. Esta revisão não altera dependências.
O pacote também traz ThirdPartyNotices.txt, que deve ser considerado em qualquer
análise de redistribuição autorizada. A auditoria dos metadados não substitui uma
auditoria de todos os binários internos desses pacotes.

Fontes primárias: License.txt e ThirdPartyNotices.txt no pacote restaurado, metadados
[NuGet da versão 18.1.0](https://www.nuget.org/packages/Microsoft.Testing.Extensions.CodeCoverage/18.1.0)
e [projeto Microsoft](https://github.com/microsoft/codecoverage).

O checkout oficial do CI permanece uma ferramenta separada, executada pelo GitHub,
e não é incorporado ao produto. Novas dependências, atualizações, publicação
self-contained ou distribuição de ferramentas exigem nova verificação de licenças
e preservação dos avisos aplicáveis; nenhum desses passos foi realizado aqui.
