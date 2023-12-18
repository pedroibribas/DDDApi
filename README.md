# DotNetDDD

## Infra

A camada de dados se divide em provedores, agrupados em uma única pasta.

Cada provedor de dados é uma biblioteca de classes própria para isolar a instalação e acesso a pacotes. Assim o uso de versões diferentes de pacotes repetidos fica flexível.

A ideia é implementar um provedor novo para cada cenário de estudo, que pode variar em linguagem ou em abordagem.
