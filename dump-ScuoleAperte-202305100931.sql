-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Versione server:              10.4.21-MariaDB - mariadb.org binary distribution
-- S.O. server:                  Win64
-- HeidiSQL Versione:            11.3.0.6295
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Dump della struttura del database scuoleaperte
CREATE DATABASE IF NOT EXISTS `scuoleaperte` /*!40100 DEFAULT CHARACTER SET utf8mb4 */;
USE `scuoleaperte`;

-- Dump della struttura di tabella scuoleaperte.appuntamenti
CREATE TABLE IF NOT EXISTS `appuntamenti` (
  `id` float NOT NULL AUTO_INCREMENT,
  `idEvento` int(11) DEFAULT NULL,
  `idUtente` int(11) DEFAULT NULL,
  `dataPrenotazione` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `appuntamenti_FK` (`idEvento`),
  KEY `appuntamenti_FK_1` (`idUtente`),
  CONSTRAINT `appuntamenti_FK` FOREIGN KEY (`idEvento`) REFERENCES `eventi` (`id`),
  CONSTRAINT `appuntamenti_FK_1` FOREIGN KEY (`idUtente`) REFERENCES `utenti` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- Dump dei dati della tabella scuoleaperte.appuntamenti: ~2 rows (circa)
/*!40000 ALTER TABLE `appuntamenti` DISABLE KEYS */;
/*!40000 ALTER TABLE `appuntamenti` ENABLE KEYS */;

-- Dump della struttura di tabella scuoleaperte.calendario
CREATE TABLE IF NOT EXISTS `calendario` (
  `data` datetime NOT NULL,
  PRIMARY KEY (`data`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Dump dei dati della tabella scuoleaperte.calendario: ~0 rows (circa)
/*!40000 ALTER TABLE `calendario` DISABLE KEYS */;
/*!40000 ALTER TABLE `calendario` ENABLE KEYS */;

-- Dump della struttura di tabella scuoleaperte.eventi
CREATE TABLE IF NOT EXISTS `eventi` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nome` varchar(100) DEFAULT NULL,
  `materia` varchar(100) DEFAULT NULL,
  `data` datetime DEFAULT NULL,
  `idOrganizzatore` int(11) DEFAULT NULL,
  `numPosti` int(11) DEFAULT NULL,
  `durata` int(11) DEFAULT NULL,
  `nPartecipanti` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `eventi_FK` (`idOrganizzatore`),
  CONSTRAINT `eventi_FK` FOREIGN KEY (`idOrganizzatore`) REFERENCES `utenti` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- Dump dei dati della tabella scuoleaperte.eventi: ~0 rows (circa)
/*!40000 ALTER TABLE `eventi` DISABLE KEYS */;
/*!40000 ALTER TABLE `eventi` ENABLE KEYS */;

-- Dump della struttura di tabella scuoleaperte.utenti
CREATE TABLE IF NOT EXISTS `utenti` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nome` varchar(100) NOT NULL,
  `cognome` varchar(100) NOT NULL,
  `mail` varchar(100) NOT NULL,
  `password` varchar(100) NOT NULL,
  `VerifiedAt` date DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `mail` (`mail`)
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8;

-- Dump dei dati della tabella scuoleaperte.utenti: ~2 rows (circa)
/*!40000 ALTER TABLE `utenti` DISABLE KEYS */;
INSERT INTO `utenti` (`id`, `nome`, `cognome`, `mail`, `password`, `VerifiedAt`) VALUES
	(23, 'luca', 'scala', 'lucascala@blabla', '$2a$11$08M91.016zlKr3s5BL67UOgzsvQNKehEtBBDXCnGOZlAY5PXGpg8.', '2023-05-15'),
	(24, 'ale', 'dina', 'aledina@blabla', '$2a$11$e3pBJgo5vWQ8eurKpAACEeHpiu1EfO/IyKZ0Zm13uI/g8wn41Zz1y', '2023-05-15');
/*!40000 ALTER TABLE `utenti` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
