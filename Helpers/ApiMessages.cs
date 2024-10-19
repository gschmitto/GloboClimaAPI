namespace GloboClimaAPI.Helpers
{
    /// <summary>
    /// Contém mensagens para respostas da API relacionadas a cidades favoritas.
    /// </summary>
    public static class ApiMessages
    {
        /// <summary>
        /// Mensagem indicando que a cidade foi adicionada aos favoritos.
        /// </summary>
        public const string CityAddedToFavorites = "Cidade adicionada aos favoritos.";

        /// <summary>
        /// Mensagem indicando que a cidade foi removida dos favoritos.
        /// </summary>
        public const string CityRemovedFromFavorites = "Cidade removida dos favoritos.";

        /// <summary>
        /// Mensagem de erro ao tentar adicionar uma cidade aos favoritos.
        /// </summary>
        public const string ErrorAddingCityToFavorites = "Erro ao adicionar cidade aos favoritos.";

        /// <summary>
        /// Mensagem de erro ao tentar remover uma cidade dos favoritos.
        /// </summary>
        public const string ErrorRemovingCityFromFavorites = "Erro ao remover cidade dos favoritos.";

        /// <summary>
        /// Mensagem indicando que o usuário não tem cidades favoritas.
        /// </summary>
        public const string NoFavoriteCities = "Você não tem cidades favoritas.";
    }
}
