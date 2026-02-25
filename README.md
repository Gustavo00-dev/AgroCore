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
       
    end

    subgraph Mensageria
        D["RabbitMQ<br>Filas:<br>- Cadastro de Propriedade<br>- Cadastro de Talhões<br>- Ingestão Sensores"]
    end

    subgraph Alertas
        F(MSAgroNotificacao)
    end

    subgraph Dados
        E["APIAgroCoreDados<br>Gerência Propriedades e Talhões<br>Consumer RabbitMQ"]
    end

    C[(MySql)]
    
    A -->|HTTPS| B
    A -->|Publica mensagens| D
    D -->|Consome mensagens| E
    E <--> C
    B <--> C
    F <--> C

```
# Arquitetura do Solução
```mermaid
flowchart LR

    %% ========================
    %% CI/CD
    %% ========================
    subgraph CICD["CI/CD - GitHub Actions"]
        DEV["Developer"]
        GH["GitHub Repository"]
        GA["GitHub Actions<br>Build Docker Image"]
        DH["DockerHub<br>Container Registry"]

        DEV --> GH
        GH --> GA
        GA --> DH
    end


    %% ========================
    %% AMBIENTE LOCAL
    %% ========================
    subgraph LOCAL["Docker Desktop"]
        Conteiner["Conteiner"]
        BD["Mysql Container"]

        MQ["RabbitMQ Container<br>Mensageria"]

        K8s["Kubernates<br>Aplicações"]

        AgroCoreOrquestradora["AgroCoreOrquestradora"]
        AgroCoreLogin["AgroCoreLogin"]
        AgroCoreDados["AgroCoreDados"]
        MSAgroNotificacao["MSAgroNotificacao"]

        Conteiner --> MQ
        Conteiner --> BD

         K8s --> AgroCoreOrquestradora
         K8s --> AgroCoreLogin
         K8s --> AgroCoreDados
         K8s --> MSAgroNotificacao
    end

```
