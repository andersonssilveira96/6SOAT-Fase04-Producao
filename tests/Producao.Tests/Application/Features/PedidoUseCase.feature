Feature: Gerenciamento de Pedidos
  Como um sistema de gestão de pedidos,
  Eu quero gerenciar os pedidos, incluindo atualizar status, listar e inserir pedidos
  Para garantir a operação correta.

  Scenario: Atualizar o status de um pedido com sucesso
    Given um pedido existente com ID "1" e status "Recebido"
    When eu atualizar o status do pedido para "EmPreparacao"
    Then o status do pedido deve ser atualizado para "EmPreparacao"

  Scenario: Falha ao atualizar o status para um valor inválido
    Given um pedido existente com ID "2" e status "Recebido"
    When eu tentar atualizar o status do pedido para "999"
    Then uma exceção deve ser lançada com a mensagem "Status 999 inválido"

  Scenario: Inserir um novo pedido com sucesso
    Given um pedido com ID "3" está pronto para ser cadastrado
    When eu inserir o pedido
    Then uma mensagem de sucesso deve ser retornada como "Pedido cadastrado com sucesso"

  Scenario: Listar todos os pedidos
    Given existem pedidos no sistema
    When eu listar os pedidos
    Then a lista de pedidos deve ser retornada
