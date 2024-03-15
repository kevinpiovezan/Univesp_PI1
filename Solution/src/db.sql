CREATE DATABASE  IF NOT EXISTS `uniceu` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `uniceu`;
-- MySQL dump 10.13  Distrib 8.0.30, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: uniceu
-- ------------------------------------------------------
-- Server version 8.3.0
 
/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
 
--
-- Table structure for table `Alunos`
--
 
DROP TABLE IF EXISTS `Alunos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Alunos` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nome` varchar(255) NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
 
--
-- Dumping data for table `Alunos`
--
 
LOCK TABLES `Alunos` WRITE;
/*!40000 ALTER TABLE `Alunos` DISABLE KEYS */;
/*!40000 ALTER TABLE `Alunos` ENABLE KEYS */;
UNLOCK TABLES;
 
--
-- Table structure for table `Listas`
--
 
DROP TABLE IF EXISTS `Listas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Listas` (
  `Nome` varchar(255) NOT NULL,
  `Data_Ultima_Atualizacao` datetime NOT NULL,
  `Id_Usuario_Alteracao` int NOT NULL,
  `Valores` text NOT NULL,
  `Id` int NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
 
--
-- Dumping data for table `Listas`
--
 
LOCK TABLES `Listas` WRITE;
/*!40000 ALTER TABLE `Listas` DISABLE KEYS */;
/*!40000 ALTER TABLE `Listas` ENABLE KEYS */;
UNLOCK TABLES;
 
--
-- Table structure for table `Tabelas`
--
 
DROP TABLE IF EXISTS `Tabelas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Tabelas` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nome` varchar(255) NOT NULL,
  `Conteudo` longtext NOT NULL,
  `Id_Usuario_Alteracao` int NOT NULL,
  `Data_Ultima_Alteracao` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
 
--
-- Dumping data for table `Tabelas`
--
 
LOCK TABLES `Tabelas` WRITE;
/*!40000 ALTER TABLE `Tabelas` DISABLE KEYS */;
/*!40000 ALTER TABLE `Tabelas` ENABLE KEYS */;
UNLOCK TABLES;
 
--
-- Table structure for table `Usuarios`
--
 
DROP TABLE IF EXISTS `Usuarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Usuarios` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Nome` varchar(255) NOT NULL,
  `Email` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
 
--
-- Dumping data for table `Usuarios`
--
 
LOCK TABLES `Usuarios` WRITE;
/*!40000 ALTER TABLE `Usuarios` DISABLE KEYS */;
INSERT INTO `Usuarios` VALUES (1,'Kevin Piovezan','kevinpiovezan@gmail.com');
/*!40000 ALTER TABLE `Usuarios` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;
 
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
 
-- Dump completed on 2024-03-14 17:05:08