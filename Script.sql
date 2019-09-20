--Criando e utilizando base de dados registroimc
CREATE DATABASE registroimc;
GO
USE registroimc;
GO

--Criando tabela Usuario
CREATE TABLE TB_Usuario (
  CodUsuario INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
  Nome VARCHAR(60) NOT NULL,
  Email VARCHAR(100) NOT NULL UNIQUE,
  Senha BINARY(64) NOT NULL
);
GO

--Criando tabela Registro
CREATE TABLE TB_Registro (
  CodRegistro INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
  Peso DECIMAL(5,2) NOT NULL,
  Altura SMALLINT NOT NULL,
  DataRegistro DATE NOT NULL,
  CodUsuario INT NOT NULL FOREIGN KEY REFERENCES TB_Usuario(CodUsuario)
);
GO

--Insert teste
/*
INSERT INTO TB_Usuario VALUES(
'Lucas',
'lucas@gmail.com',
HASHBYTES('SHA2_256','123')
);

INSERT INTO TB_Registro VALUES(
84.53,
188.22,
1
);
*/

--Select teste
SELECT * FROM TB_Usuario;
SELECT * FROM TB_Registro;