/*
SQLyog Community v13.1.7 (64 bit)
MySQL - 10.4.21-MariaDB : Database - penitipan_abu_adijasa
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`penitipan_abu_adijasa` /*!40100 DEFAULT CHARACTER SET utf8mb4 */;

USE `penitipan_abu_adijasa`;

/*Table structure for table `data_abu` */

DROP TABLE IF EXISTS `data_abu`;

CREATE TABLE `data_abu` (
  `id` int(255) NOT NULL AUTO_INCREMENT,
  `nama_abu` varchar(255) NOT NULL,
  `nama_alternatif_abu` varchar(255) DEFAULT NULL,
  `alamat_abu` varchar(255) NOT NULL,
  `jenis_kelamin` varchar(25) NOT NULL,
  `tanggal_lahir` date NOT NULL,
  `tanggal_wafat` date NOT NULL,
  `tanggal_kremasi` date NOT NULL,
  `keterangan` longtext DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;

/*Table structure for table `jaminan` */

DROP TABLE IF EXISTS `jaminan`;

CREATE TABLE `jaminan` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_penitipan` varchar(255) NOT NULL,
  `total_jaminan` int(11) NOT NULL,
  `status` int(1) NOT NULL DEFAULT 0 COMMENT '0 = belum terbayar, 1 = sudah terbayar',
  `dikembalikan` int(1) NOT NULL DEFAULT 0 COMMENT '0 = belum, 1 = sudah',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4;

/*Table structure for table `kategori` */

DROP TABLE IF EXISTS `kategori`;

CREATE TABLE `kategori` (
  `id` int(255) NOT NULL AUTO_INCREMENT,
  `nama` varchar(150) NOT NULL,
  `harga` int(255) NOT NULL,
  `status` int(1) DEFAULT 0 COMMENT '0 == Not Deleted, 1 == Deleted',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;

/*Table structure for table `kotak` */

DROP TABLE IF EXISTS `kotak`;

CREATE TABLE `kotak` (
  `id` int(255) NOT NULL AUTO_INCREMENT,
  `kategori_id` int(255) NOT NULL,
  `no_kotak` varchar(100) NOT NULL,
  `status` int(1) DEFAULT 0 COMMENT '0 == Not Deleted, 1 == Deleted',
  `terpakai` int(1) DEFAULT 0 COMMENT '0 == Not used, 1 == Used',
  `booking` int(1) DEFAULT 0 COMMENT '0 == Not booked, 1 == Booked',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4;

/*Table structure for table `pembayaran_sewa` */

DROP TABLE IF EXISTS `pembayaran_sewa`;

CREATE TABLE `pembayaran_sewa` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_penitipan` int(11) NOT NULL,
  `id_kotak` int(11) NOT NULL,
  `harga_kotak` int(11) NOT NULL,
  `harga_total_sewa` int(11) NOT NULL,
  `tanggal_awal` date NOT NULL,
  `tanggal_akhir` date NOT NULL,
  `status` int(1) NOT NULL DEFAULT 0,
  `tanggal_diambil` date DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `id_penitipan` (`id_penitipan`),
  KEY `id_kotak` (`id_kotak`),
  CONSTRAINT `pembayaran_sewa_ibfk_1` FOREIGN KEY (`id_penitipan`) REFERENCES `penitipan` (`id`),
  CONSTRAINT `pembayaran_sewa_ibfk_2` FOREIGN KEY (`id_kotak`) REFERENCES `kotak` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4;

/*Table structure for table `penanggung_jawab` */

DROP TABLE IF EXISTS `penanggung_jawab`;

CREATE TABLE `penanggung_jawab` (
  `id` int(255) NOT NULL AUTO_INCREMENT,
  `nama` varchar(255) NOT NULL,
  `alamat` varchar(255) NOT NULL,
  `nomor_telp` varchar(55) NOT NULL,
  `relasi` varchar(150) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4;

/*Table structure for table `pengambilan_abu` */

DROP TABLE IF EXISTS `pengambilan_abu`;

CREATE TABLE `pengambilan_abu` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_penitipan` int(11) NOT NULL,
  `tanggal_pengambilan` date NOT NULL,
  `status` int(1) NOT NULL DEFAULT 0 COMMENT '0 = deleted, 1 = not-deleted',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4;

/*Table structure for table `penitipan` */

DROP TABLE IF EXISTS `penitipan`;

CREATE TABLE `penitipan` (
  `id` int(255) NOT NULL AUTO_INCREMENT,
  `tanggal_registrasi` date NOT NULL,
  `tanggal_titip` date DEFAULT NULL,
  `tanggal_ambil` date DEFAULT NULL,
  `kotak_id` int(255) NOT NULL,
  `data_abu_id` int(11) NOT NULL,
  `penanggung_jawab_satu_id` int(11) NOT NULL,
  `penanggung_jawab_dua_id` int(11) DEFAULT NULL,
  `status` int(1) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4;

/*Table structure for table `users` */

DROP TABLE IF EXISTS `users`;

CREATE TABLE `users` (
  `id` int(255) NOT NULL AUTO_INCREMENT,
  `username` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `role` int(1) NOT NULL COMMENT '1 = admin, 0 = staff',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
