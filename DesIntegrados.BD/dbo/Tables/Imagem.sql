CREATE TABLE [dbo].[Imagem] (
    [ID]           INT             IDENTITY (1, 1) NOT NULL,
    [Id_Usuario]   INT             NULL,
    [Tamanho]      INT             NULL,
    [Iteracoes]    INT             NULL,
    [DataInicio]   DATETIME        NULL,
    [DataFim]      DATETIME        NULL,
    [Reconstruida] VARBINARY (MAX) NULL
);

