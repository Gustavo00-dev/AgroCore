# AgroCore
AgroCore - O núcleo inteligente da agricultura de precisão.


# Fluxo de comunicação entre Microserviços
```mermaid
flowchart LR

    subgraph Orquestracao
        A["AgroCoreOrquestradora<br>API Orquestradora"]
    end

    subgraph Autenticacao
        B["APIAgroCoreLogin<br>Gerência Token JWT<br>Login de Usuários"]
        C[(MongoDB)]
        B <--> C
    end

    subgraph Mensageria
        D["RabbitMQ<br>Filas:<br>- Cadastro de Propriedade<br>- Cadastro de Talhões<br>- Ingestão Sensores"]
    end

    subgraph Dados
        E["APIAgroCoreDados<br>Gerência Propriedades e Talhões<br>Consumer RabbitMQ"]
    end

    A -->|HTTPS| B
    A -->|Publica mensagens| D
    D -->|Consome mensagens| E
    E <--> C

```
