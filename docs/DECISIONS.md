# Decisões

1. .NET 10 com SDK estável e rollForward latestFeature: acompanha correções da
   linha 10.0 sem aceitar previews ou uma versão principal posterior.
2. Três projetos: composição/apresentação, domínio/adaptador e testes.
3. MSTest 4.0.2 da Microsoft, fornecido pelo template do SDK instalado:
   sem frameworks de testes de terceiros ou pacotes de runtime.
4. Sem WMI: APIs do framework e uma função Win32 bastam para o escopo.
5. Falha por campo retorna indisponível e exit code 1; uso incorreto retorna 2.
6. Formatação de memória em GiB com duas casas e cultura invariável para saída previsível.
7. CI com action oficial actions/checkout fixada por SHA, permissões globais vazias
   e apenas contents: read no job. O token temporário não é persistido no Git;
   checkout suporta o próprio repositório público ou privado, sem secrets personalizados.
   Referência: [action oficial](https://github.com/actions/checkout/tree/11d5960a326750d5838078e36cf38b85af677262).
   O runner precisa ter SDK .NET 10; ausência causa falha explícita, sem instalação automática.
8. CORE/STUDIO, mutações do sistema e distribuição ficam
   para decisões futuras do mantenedor.
9. Decisão do mantenedor: código BigzOS sob GPL-3.0-only, exclusivamente versão 3,
   sem termos adicionais. LICENSE preserva o texto oficial integral da GNU.
   Metadados NuGet e de assembly usam o identificador SPDX; componentes externos
   mantêm suas licenças. Auditoria das dependências em docs/LICENSING.md.

