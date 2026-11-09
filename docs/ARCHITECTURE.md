# Arquitetura

BigzOS.Cli → BigzOS.Core. BigzOS.Tests referencia ambos.

ISystemInformationSource expõe quatro leituras independentes. O adaptador
WindowsInformationSource usa RuntimeInformation para versão e arquitetura,
Microsoft.Win32.Registry com writable: false para o nome do processador e
GlobalMemoryStatusEx para memória física total. Não usa WMI ou pacotes de runtime.

SystemInformationCollector converte cada falha em InformationField indisponível.
Uma falha não interrompe os outros campos. OutOfMemoryException não é mascarada.
Não se exibem detalhes internos de exceções. SystemInformation é um snapshot imutável.

CliApplication recebe fábrica da fonte, escritores e identificação da plataforma.
Somente Program consulta a plataforma real e compõe o adaptador. Argumentos e
plataforma são validados antes de criar a fonte. Testes substituem todas as leituras.

Memória representa o total físico utilizável informado pelo Windows, podendo ser
menor que a capacidade instalada por reservas de hardware. Arquitetura representa
o SO, não necessariamente o processo. Versão técnica não é edição comercial.

Sem contêiner de DI, parser externo ou camadas adicionais. CORE e STUDIO não
possuem implementação ou comportamento nesta fase.

