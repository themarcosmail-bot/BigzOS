# Segurança

O MVP executa como usuário comum e apenas lê informações do sistema.
Não altera configurações, não cria tarefas/serviços, não executa shell ou comandos
externos e não usa rede, telemetria, identificadores de usuário ou credenciais.

O registro é aberto explicitamente para leitura em
HKLM/HARDWARE/DESCRIPTION/System/CentralProcessor/0. A memória é consultada via
GlobalMemoryStatusEx. Falhas são exibidas como indisponibilidade por campo,
sem stack traces ou detalhes internos na saída normal.

A saída contém características de hardware: revise antes de compartilhar.
Chamadas nativas são restritas ao Windows; testes não as executam.

Dependências de desenvolvimento: MSTest Microsoft e suas dependências transitivas.
Restore requer acesso ao NuGet; isso não faz parte da execução do produto.
O SDK pode ter efeitos de primeiro uso; use as variáveis do README, incluindo
DOTNET_GENERATE_ASPNET_CERTIFICATE=false, antes do primeiro comando.
TESTINGPLATFORM_TELEMETRY_OPTOUT=1 é definido no ambiente de testes e no CI
para desativar a telemetria da Microsoft.Testing.Platform quando aplicável.
Não desativa toda a telemetria do SDK .NET; DOTNET_CLI_TELEMETRY_OPTOUT=1
é configurado separadamente. As variáveis são locais ao processo e seus filhos.

CI usa permissions: {} no nível global e somente contents: read no job,
com runner hospedado descartável. A action oficial actions/checkout é fixada por
SHA verificado no repositório actions/checkout. O token temporário github.token
permite checkout do próprio repositório público ou privado. persist-credentials: false
impede sua persistência no Git; clean: false evita limpeza destrutiva e
set-safe-directory: false evita escrita na configuração global do Git.
Não referencia secrets personalizados, actions de terceiros ou pull_request_target.
Build/testes não recebem o token por argumentos ou variáveis de ambiente do projeto.
Nunca mover esse job para runner próprio com privilégios ou dados sensíveis. Acesso de rede do runner para GitHub/NuGet
não significa acesso de rede do produto.

Não há garantia de disponibilidade de todos os campos em ambientes restritos.
Reporte problemas com reprodução mínima, removendo dados pessoais da saída.
Não há canal privado de vulnerabilidades configurado ainda.

