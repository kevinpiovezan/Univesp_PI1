DROP TABLE IF EXISTS `Alunos`;
CREATE TABLE `Alunos` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Id_Status_Matricula` int NOT NULL,
  `Nome` varchar(255) NOT NULL,
  `RA` varchar(255) NOT NULL,
  `Nome_Social` varchar(255) DEFAULT NULL,
  `Cep` varchar(26) NOT NULL,
  `Endereco` text NOT NULL,
  `Cursou_Faculdade` tinyint(1) NOT NULL,
  `Cursos` text,
  `Professor` tinyint(1) NOT NULL,
  `Eixo` varchar(255) NOT NULL,
  `Cadastro_Sptrans` tinyint(1) NOT NULL,
  `Autorizacao_Imagem` tinyint(1) NOT NULL,
  `Genero` varchar(26) NOT NULL,
  `Raca_Cor_Etnia` varchar(26) NOT NULL,
  `EnsinoMedio_Escola_Publica` tinyint(1) NOT NULL,
  `Data_Nascimento` date NOT NULL,
  `Cpf` varchar(26) NOT NULL,
  `Rg` varchar(26) NOT NULL,
  `UF` varchar(2) NOT NULL,
  `Data_Emissao` date NOT NULL,
  `Email` varchar(255) NOT NULL,
  `Celular` varchar(26) NOT NULL,
  `Tel_Fixo` varchar(26) DEFAULT NULL,
  `Ultima_Atualizacao` datetime NOT NULL,
  `Servidor_Publico` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
LOCK TABLES `Alunos` WRITE;
UNLOCK TABLES;
 
DROP TABLE IF EXISTS `Listas`;
CREATE TABLE `Listas` (
  `Nome` varchar(255) NOT NULL,
  `Data_Ultima_Atualizacao` datetime NOT NULL,
  `Id_Usuario_Alteracao` int NOT NULL,
  `Valores` text NOT NULL,
  `Id` int NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

LOCK TABLES `Listas` WRITE;

UNLOCK TABLES;
DROP TABLE IF EXISTS `Tabelas`;
CREATE TABLE `Tabelas` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nome` varchar(255) NOT NULL,
  `Conteudo` longtext NOT NULL,
  `Id_Usuario_Alteracao` int NOT NULL,
  `Data_Ultima_Alteracao` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
LOCK TABLES `Tabelas` WRITE;
UNLOCK TABLES;


DROP TABLE IF EXISTS `Usuarios`;
CREATE TABLE `Usuarios` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Nome` varchar(255) NOT NULL,
  `Email` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
LOCK TABLES `Usuarios` WRITE;
INSERT INTO `Usuarios` VALUES (1,'Kevin Piovezan','kevinpiovezan@gmail.com');
UNLOCK TABLES;