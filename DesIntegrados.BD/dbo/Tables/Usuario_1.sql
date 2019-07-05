CREATE TABLE [dbo].[Usuario] (
    [ID]         INT           IDENTITY (1, 1) NOT NULL,
    [nome]       VARCHAR (255) NULL,
    [nascimento] DATETIME      NULL,
    [email]      VARCHAR (255) NULL,
    [senha]      VARCHAR (255) NULL,
    [Datafim]    DATETIME      NULL,
    [DataInicio] DATETIME      NULL,
    [tamanho]    VARCHAR (15)  NULL,
    [iteracoes]  INT           NULL
);

