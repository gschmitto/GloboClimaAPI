using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace GloboClimaAPI.Infrastructure.Data
{
    /// <summary>
    /// Classe responsável pela inicialização e criação de tabelas no DynamoDB.
    /// </summary>
    public class DynamoDBInitializer
    {
        private readonly IAmazonDynamoDB _dynamoDBClient;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="DynamoDBInitializer"/> com o cliente DynamoDB injetado.
        /// </summary>
        /// <param name="dynamoDBClient">Instância de IAmazonDynamoDB para interagir com o DynamoDB.</param>
        public DynamoDBInitializer(IAmazonDynamoDB dynamoDBClient)
        {
            _dynamoDBClient = dynamoDBClient;
        }

        /// <summary>
        /// Método responsável por criar as tabelas necessárias no DynamoDB, incluindo as tabelas de Usuários e Favoritos.
        /// </summary>
        /// <returns>Task assíncrona representando o processo de criação das tabelas.</returns>
        public async Task InitializeTablesAsync()
        {
            // Criar a tabela de usuários
            await CreateTableIfNotExistsAsync("Users", new List<KeySchemaElement>
            {
                new KeySchemaElement("Id", KeyType.HASH)
            }, new List<AttributeDefinition>
            {
                new AttributeDefinition("Id", ScalarAttributeType.S),
                new AttributeDefinition("Email", ScalarAttributeType.S)
            }, new List<GlobalSecondaryIndex>
            {
                new GlobalSecondaryIndex
                {
                    IndexName = "EmailIndex",
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement("Email", KeyType.HASH)
                    },
                    Projection = new Projection
                    {
                        ProjectionType = ProjectionType.ALL
                    },
                    ProvisionedThroughput = new ProvisionedThroughput(5, 5) // Definindo throughput
                }
            });

            // Criar a tabela de favoritos
            await CreateTableIfNotExistsAsync("UserFavorites", new List<KeySchemaElement>
            {
                new KeySchemaElement("Email", KeyType.HASH)
            }, new List<AttributeDefinition>
            {
                new AttributeDefinition("Email", ScalarAttributeType.S)
            });
        }

        /// <summary>
        /// Cria uma tabela no DynamoDB se ela não existir.
        /// </summary>
        /// <param name="tableName">Nome da tabela a ser criada.</param>
        /// <param name="keySchema">Esquema de chaves primárias (HASH e RANGE).</param>
        /// <param name="attributeDefinitions">Definições dos atributos da tabela.</param>
        /// <param name="globalSecondaryIndexes">Índices globais secundários (opcional).</param>
        /// <returns>Task assíncrona representando o processo de criação da tabela.</returns>
        private async Task CreateTableIfNotExistsAsync(string tableName, List<KeySchemaElement> keySchema, List<AttributeDefinition> attributeDefinitions, List<GlobalSecondaryIndex>? globalSecondaryIndexes = null)
        {
            // Verifica se a tabela já existe
            var listTablesResponse = await _dynamoDBClient.ListTablesAsync();
            if (listTablesResponse.TableNames.Contains(tableName))
            {
                Console.WriteLine($"A tabela {tableName} já existe. Pulando a criação.");
                return;
            }

            // Cria a tabela
            var tableRequest = new CreateTableRequest
            {
                TableName = tableName,
                KeySchema = keySchema,
                AttributeDefinitions = attributeDefinitions,
                ProvisionedThroughput = new ProvisionedThroughput(5, 5),
                GlobalSecondaryIndexes = globalSecondaryIndexes
            };

            var response = await _dynamoDBClient.CreateTableAsync(tableRequest);
            Console.WriteLine($"Table {tableName} creation response: {response.HttpStatusCode}");
        }
    }
}
