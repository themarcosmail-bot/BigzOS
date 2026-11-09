# Revisão com Claude Code

Leia AGENTS.md e siga suas restrições. O escopo aprovado é a fundação do
Bigz Toolkit, sem aplicar personalizações ao Windows.

Revise:
- separação entre ISystemInformationSource, coletor e apresentação da CLI;
- leitura exclusiva no adaptador Windows e layout da estrutura nativa;
- falhas independentes, mensagens e códigos de saída;
- testes determinísticos sem consultas reais;
- ausência de rede, telemetria e elevação no produto;
- workflow com checkout oficial fixado por SHA, contents: read e sem persistência
  de credenciais, secrets personalizados ou actions de terceiros;
- resultados e pendências em tasks/CURRENT.md.

Use os comandos e variáveis de ambiente documentados no README.
Não faça commit, push, instalação externa ou mudanças no sistema sem autorização.

