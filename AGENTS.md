# Instruções para agentes

- Trabalhe somente neste repositório; leia README.md e docs antes de mudar o escopo.
- Não modifique o Windows, registro, serviços, Defender, UAC, firewall ou Windows Update.
- Não solicite elevação, não adicione telemetria nem rede ao MVP.
- Coleta deve ser somente leitura e isolada da CLI por abstrações testáveis.
- Prefira APIs oficiais .NET 10; peça autorização antes de dependências externas.
- Não instale dependências sem autorização. Restore de dependências oficiais
  necessárias foi autorizado nesta implementação.
- Não faça commit, push ou operações destrutivas sem aprovação explícita.
- Configure as variáveis de inicialização do SDK do README antes de usar dotnet.
- Testes unitários devem usar fontes simuladas, sem depender da máquina real.
- Execute build e testes após mudanças relevantes e registre limitações.
- CI: permissões globais vazias e somente contents: read no job para checkout oficial
  fixado por SHA, sem persistência de credenciais, secrets personalizados, deploy,
  pull_request_target ou execução privilegiada.
- Mantenha três projetos, arquitetura simples e documentação coerente.

