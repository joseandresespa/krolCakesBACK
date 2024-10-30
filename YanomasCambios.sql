-- MySQL dump 10.13  Distrib 8.0.39, for Win64 (x86_64)
--
-- Host: localhost    Database: carolcakes
-- ------------------------------------------------------
-- Server version	8.0.39

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `acceso`
--

DROP TABLE IF EXISTS `acceso`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `acceso` (
  `correlativo` int NOT NULL AUTO_INCREMENT COMMENT 'En este campo almacenaremos el correlativo de cada acceso.',
  `id_rol` int NOT NULL COMMENT 'En este campo almacenaremos el identificador del rol para su acceso.',
  `id_modulo` int NOT NULL COMMENT 'En este campo alamcenaremos el identificador para el acceso al modulo.',
  PRIMARY KEY (`correlativo`),
  KEY `fk_acceso_rol_idx` (`id_rol`),
  KEY `fk_acceso_modulo1_idx` (`id_modulo`),
  CONSTRAINT `fk_acceso_modulo1` FOREIGN KEY (`id_modulo`) REFERENCES `modulo` (`id`),
  CONSTRAINT `fk_acceso_rol` FOREIGN KEY (`id_rol`) REFERENCES `rol` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `acceso`
--

LOCK TABLES `acceso` WRITE;
/*!40000 ALTER TABLE `acceso` DISABLE KEYS */;
/*!40000 ALTER TABLE `acceso` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cliente`
--

DROP TABLE IF EXISTS `cliente`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cliente` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'Se almacena el identificador del cliente.',
  `nombre` varchar(200) NOT NULL COMMENT 'Se almacena el nombre del cliente.',
  `telefono` int NOT NULL COMMENT 'Se almacena el telefono del cliente.',
  `nit` varchar(45) NOT NULL COMMENT 'Se almacena el nit del cliente.',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cliente`
--

LOCK TABLES `cliente` WRITE;
/*!40000 ALTER TABLE `cliente` DISABLE KEYS */;
INSERT INTO `cliente` VALUES (3,'Jose',1234678,'123456-7'),(4,'Jose',12345679,''),(5,'prueba completa nombre',456,''),(6,'Prueba Cotizacion',123,''),(7,'Prueba Cotizacion nueva para imagenes',4567,'');
/*!40000 ALTER TABLE `cliente` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `compra_inventario`
--

DROP TABLE IF EXISTS `compra_inventario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `compra_inventario` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'Se almacena el identificador de la compra que se realice al inventario.',
  `total` double NOT NULL COMMENT 'Se almacena el total de la compra que se realice.',
  `fecha_compra` date DEFAULT NULL COMMENT 'Se almacena la fecha en la que se realizo la compra.',
  `id_proveedor` int NOT NULL COMMENT 'Se almacena el identificador del proveedor al que se le hizo la compra.',
  PRIMARY KEY (`id`),
  KEY `fk_compra_inventario_proveedor1_idx` (`id_proveedor`),
  CONSTRAINT `fk_compra_inventario_proveedor1` FOREIGN KEY (`id_proveedor`) REFERENCES `proveedor` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `compra_inventario`
--

LOCK TABLES `compra_inventario` WRITE;
/*!40000 ALTER TABLE `compra_inventario` DISABLE KEYS */;
INSERT INTO `compra_inventario` VALUES (1,100,'2024-10-10',3),(2,50,'2024-11-11',4),(3,20,'2024-10-24',4),(4,20,'2024-10-24',4),(5,20,'2024-10-24',4);
/*!40000 ALTER TABLE `compra_inventario` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `costo`
--

DROP TABLE IF EXISTS `costo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `costo` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'Se almacena el identificador del costo.',
  `id_pedido` int NOT NULL COMMENT 'Se almacena a que pedido se le esta realizando el costo.',
  `costo` double DEFAULT NULL COMMENT 'Se almacena el costo del pedido.',
  `ganancia` double DEFAULT NULL COMMENT 'Se almacena la ganacia del pedido.',
  PRIMARY KEY (`id`),
  KEY `fk_costo_pedido1_idx` (`id_pedido`),
  CONSTRAINT `fk_costo_pedido1` FOREIGN KEY (`id_pedido`) REFERENCES `pedido` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `costo`
--

LOCK TABLES `costo` WRITE;
/*!40000 ALTER TABLE `costo` DISABLE KEYS */;
INSERT INTO `costo` VALUES (3,3,10,90);
/*!40000 ALTER TABLE `costo` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cotizacion_online`
--

DROP TABLE IF EXISTS `cotizacion_online`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cotizacion_online` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'Se almacena el identificador de la cotización online.',
  `descripcion` varchar(500) NOT NULL COMMENT 'Almacena una breve descripción de la cotización.',
  `precio_aproximado` double DEFAULT NULL COMMENT 'Indica el precio aproximado de la cotización.',
  `envio` tinyint DEFAULT NULL COMMENT 'Indica si quiere envio o no.',
  `hora` time NOT NULL,
  `fecha` date DEFAULT NULL,
  `direccion` varchar(255) DEFAULT NULL,
  `estado` int DEFAULT NULL,
  `mano_obra` double DEFAULT NULL,
  `presupuesto_insumos` double DEFAULT NULL,
  `total_presupuesto` double DEFAULT NULL,
  `cliente_id` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_cotizacion_online_cliente1_idx` (`cliente_id`),
  KEY `fkEstado` (`estado`),
  CONSTRAINT `fk_cotizacion_online_cliente1` FOREIGN KEY (`cliente_id`) REFERENCES `cliente` (`id`),
  CONSTRAINT `fkEstado` FOREIGN KEY (`estado`) REFERENCES `estado` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cotizacion_online`
--

LOCK TABLES `cotizacion_online` WRITE;
/*!40000 ALTER TABLE `cotizacion_online` DISABLE KEYS */;
INSERT INTO `cotizacion_online` VALUES (12,'prueba completa descripcion',110,1,'20:22:00','2024-10-30','en mi casa ',1,40,60,100,5),(13,'Prueba de nuevo pedido para imagenes',140,1,'20:47:00','2024-11-01','Casa 3',7,NULL,NULL,NULL,6),(14,'Prueba de nuevo pedido para imagenes',140,1,'20:47:00','2024-11-01','Casa 3',7,NULL,NULL,NULL,7);
/*!40000 ALTER TABLE `cotizacion_online` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `desglose_online`
--

DROP TABLE IF EXISTS `desglose_online`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `desglose_online` (
  `correlativo` int NOT NULL AUTO_INCREMENT COMMENT 'Se almacena el correlativo del desglose online.',
  `id_cotizacion_online` int NOT NULL COMMENT 'identificador de la cotizacion al que pertence el desgloce.',
  `id_producto` int NOT NULL COMMENT 'identificador del producto  al que pertence el desgloce.',
  `subtotal` double NOT NULL,
  `cantidad` int NOT NULL,
  `precio_pastelera` double DEFAULT NULL,
  PRIMARY KEY (`correlativo`),
  KEY `fk_desglose_online_producto1_idx` (`id_producto`),
  KEY `fk_desglose_online_contizacion_onliine1_idx` (`id_cotizacion_online`),
  CONSTRAINT `fk_desglose_online_contizacion_onliine1` FOREIGN KEY (`id_cotizacion_online`) REFERENCES `cotizacion_online` (`id`),
  CONSTRAINT `fk_desglose_online_producto1` FOREIGN KEY (`id_producto`) REFERENCES `producto` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `desglose_online`
--

LOCK TABLES `desglose_online` WRITE;
/*!40000 ALTER TABLE `desglose_online` DISABLE KEYS */;
INSERT INTO `desglose_online` VALUES (12,12,4,50,2,NULL),(13,12,5,60,2,NULL),(14,14,4,50,2,NULL),(15,14,5,90,3,NULL);
/*!40000 ALTER TABLE `desglose_online` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `detalle_costo`
--

DROP TABLE IF EXISTS `detalle_costo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `detalle_costo` (
  `correlativo` int NOT NULL AUTO_INCREMENT COMMENT 'Se almacena el correlativo del detalle costo.',
  `id_costo` int NOT NULL COMMENT 'Se almacena la llave foranea del costo del pedido.',
  `id_insumo_utensilio` int NOT NULL COMMENT 'Se almacena la llave foranea del insumo utensilio a usar.',
  `cantidad` double NOT NULL COMMENT 'Almacena la cantidad que se utilizara para preparar el pedido.',
  `id_unidad_medida_ps` int NOT NULL,
  PRIMARY KEY (`correlativo`),
  KEY `fk_detalle_costo_costo1_idx` (`id_costo`),
  KEY `fk_detalle_costo_insumo_utensilio1_idx` (`id_insumo_utensilio`),
  KEY `fk1_idx` (`id_unidad_medida_ps`),
  CONSTRAINT `fk1` FOREIGN KEY (`id_unidad_medida_ps`) REFERENCES `unidad_medida_precio_sugerido` (`id`),
  CONSTRAINT `fk_detalle_costo_costo1` FOREIGN KEY (`id_costo`) REFERENCES `costo` (`id`),
  CONSTRAINT `fk_detalle_costo_insumo_utensilio1` FOREIGN KEY (`id_insumo_utensilio`) REFERENCES `insumo_utensilio` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `detalle_costo`
--

LOCK TABLES `detalle_costo` WRITE;
/*!40000 ALTER TABLE `detalle_costo` DISABLE KEYS */;
INSERT INTO `detalle_costo` VALUES (5,3,3,1,2);
/*!40000 ALTER TABLE `detalle_costo` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `detalle_pedido`
--

DROP TABLE IF EXISTS `detalle_pedido`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `detalle_pedido` (
  `correlativo` int NOT NULL AUTO_INCREMENT COMMENT 'Se almacena el correlativo del detalle del pedido.',
  `id_pedido` int NOT NULL COMMENT 'Se almacena la llave foranea del pedido.',
  `producto_id` int NOT NULL COMMENT 'Se almacena la llave foranea del producto.',
  `id_masas` int NOT NULL COMMENT 'Se almacena la llave foranea de las masas.',
  `id_relleno` int NOT NULL COMMENT 'Se almacena la llave foranea del relleno.',
  `cantidad_porciones` int NOT NULL COMMENT 'Almacena la cantidad de porciones que tendra el pedido.',
  `precio_unitario` double NOT NULL COMMENT 'Almacena el precio unitario de cada porcion del pedido.',
  `total` double DEFAULT NULL COMMENT 'Almacena el precio total del pedido.',
  PRIMARY KEY (`correlativo`),
  KEY `fk_detalle_pedido_pedido1_idx` (`id_pedido`),
  KEY `fk_detalle_pedido_producto1_idx` (`producto_id`),
  KEY `fk_detalle_pedido_masas1_idx` (`id_masas`),
  KEY `fk_detalle_pedido_relleno1_idx` (`id_relleno`),
  CONSTRAINT `fk_detalle_pedido_masas1` FOREIGN KEY (`id_masas`) REFERENCES `masas` (`id`),
  CONSTRAINT `fk_detalle_pedido_pedido1` FOREIGN KEY (`id_pedido`) REFERENCES `pedido` (`id`),
  CONSTRAINT `fk_detalle_pedido_producto1` FOREIGN KEY (`producto_id`) REFERENCES `producto` (`id`),
  CONSTRAINT `fk_detalle_pedido_relleno1` FOREIGN KEY (`id_relleno`) REFERENCES `relleno` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `detalle_pedido`
--

LOCK TABLES `detalle_pedido` WRITE;
/*!40000 ALTER TABLE `detalle_pedido` DISABLE KEYS */;
INSERT INTO `detalle_pedido` VALUES (4,3,4,6,5,2,25,50),(5,3,5,7,6,2,25,50);
/*!40000 ALTER TABLE `detalle_pedido` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `encabezado_salida`
--

DROP TABLE IF EXISTS `encabezado_salida`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `encabezado_salida` (
  `id_salida` int NOT NULL AUTO_INCREMENT,
  `fecha` date NOT NULL,
  `notas` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id_salida`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `encabezado_salida`
--

LOCK TABLES `encabezado_salida` WRITE;
/*!40000 ALTER TABLE `encabezado_salida` DISABLE KEYS */;
INSERT INTO `encabezado_salida` VALUES (1,'2024-10-01','salida1'),(2,'2024-10-02','salida2'),(3,'2024-10-10','Prueba Salida'),(4,'2024-10-10','Prueba Salida'),(5,'2024-10-16','prueba');
/*!40000 ALTER TABLE `encabezado_salida` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `estado`
--

DROP TABLE IF EXISTS `estado`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `estado` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'Se almacena el id del estado.',
  `estado` varchar(45) NOT NULL COMMENT 'Almacena el estado que pueda tener el pedido.',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `estado`
--

LOCK TABLES `estado` WRITE;
/*!40000 ALTER TABLE `estado` DISABLE KEYS */;
INSERT INTO `estado` VALUES (1,'confirmados'),(3,'En Proceso'),(4,'Cancelado'),(5,'Visto'),(6,'No Confirmado'),(7,'No Visto'),(8,'Entregado al repartidor'),(9,'terminado'),(10,'Finalizado y costeado');
/*!40000 ALTER TABLE `estado` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `imagen_referencia_online`
--

DROP TABLE IF EXISTS `imagen_referencia_online`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `imagen_referencia_online` (
  `correlativo` int NOT NULL AUTO_INCREMENT COMMENT 'Se almacena el correlativo de la imagen de referencia.',
  `id_cotizacion_online` int NOT NULL COMMENT 'Se almacena la llave foranea de la cotización online.',
  `ruta` varchar(500) NOT NULL COMMENT 'Almancena la ruta donde se encuentra la imagen.',
  `observacion` varchar(255) DEFAULT NULL COMMENT 'Se almacenan las observaciones que el cliente pueda dar sobre la imagen.',
  PRIMARY KEY (`correlativo`),
  KEY `fk_imagen_referencia_en_linea_contizacion_en_linea1_idx` (`id_cotizacion_online`),
  CONSTRAINT `fk_imagen_referencia_en_linea_contizacion_en_linea1` FOREIGN KEY (`id_cotizacion_online`) REFERENCES `cotizacion_online` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `imagen_referencia_online`
--

LOCK TABLES `imagen_referencia_online` WRITE;
/*!40000 ALTER TABLE `imagen_referencia_online` DISABLE KEYS */;
INSERT INTO `imagen_referencia_online` VALUES (9,14,'https://firebasestorage.googleapis.com/v0/b/subida-ab43b.appspot.com/o/images%2FCopia%20de%20Copia%20de%20Cartel%20P%C3%B3ster%20Datos%20sobre%20el%20Proceso%20Art%C3%ADstico%20Doodle%20Amarillo%20y%20Marr%C3%B3n.png?alt=media&token=e121e296-c61e-4d46-8351-bbfd237bca7a',' '),(10,14,'https://firebasestorage.googleapis.com/v0/b/subida-ab43b.appspot.com/o/images%2Fpastel%20ref2.jpg?alt=media&token=70ac1abe-fe61-4c1d-9172-00fdf4920ce6',' ');
/*!40000 ALTER TABLE `imagen_referencia_online` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ingreso_inventario`
--

DROP TABLE IF EXISTS `ingreso_inventario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ingreso_inventario` (
  `correlativo` int NOT NULL AUTO_INCREMENT COMMENT 'Correlativo incremental de ingresos de un producto al inventario',
  `id_insumo_utensilio` int NOT NULL COMMENT 'el id de la tabla catalogo de insumos o utensilios',
  `cantidad` int NOT NULL COMMENT 'Cantidad que ingresa de un utensiolio o insumo',
  `precio_unitario` double NOT NULL COMMENT 'Precio Unitario',
  `id_compra_inventario` int NOT NULL COMMENT 'llave foranea de la compra total del detalle de la compra al proveedor',
  `subtotal` double NOT NULL COMMENT 'Subtotal de catidad * precio unitario',
  PRIMARY KEY (`correlativo`),
  KEY `fk_ingreso_inventario_insumo_utensilio1_idx` (`id_insumo_utensilio`),
  KEY `fk_ingreso_inventario_compra_inventario1_idx` (`id_compra_inventario`),
  CONSTRAINT `fk_ingreso_inventario_compra_inventario1` FOREIGN KEY (`id_compra_inventario`) REFERENCES `compra_inventario` (`id`),
  CONSTRAINT `fk_ingreso_inventario_insumo_utensilio1` FOREIGN KEY (`id_insumo_utensilio`) REFERENCES `insumo_utensilio` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ingreso_inventario`
--

LOCK TABLES `ingreso_inventario` WRITE;
/*!40000 ALTER TABLE `ingreso_inventario` DISABLE KEYS */;
INSERT INTO `ingreso_inventario` VALUES (1,3,1,1,1,10),(2,4,1,1,1,12),(3,5,1,1,2,21),(4,5,1,1,2,22),(5,3,1,15,5,15),(6,3,1,15,4,15),(7,4,2,2.5,5,5),(8,4,2,2.5,4,5);
/*!40000 ALTER TABLE `ingreso_inventario` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `insumo_utensilio`
--

DROP TABLE IF EXISTS `insumo_utensilio`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `insumo_utensilio` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'id unico del insumo utensiolio que representa el inventario',
  `id_tipo_insumo` int NOT NULL COMMENT 'llave foranea de tipo de insumo',
  `nombre` varchar(150) NOT NULL COMMENT 'nombre del insumo o utensilio',
  `id_unidad_medida` int NOT NULL COMMENT 'id unidad de medida que tiene el insumo utensilio',
  `precio_unitario` double DEFAULT NULL COMMENT 'precio unitario del insumo utensilio',
  `cantidad` int DEFAULT NULL COMMENT 'cantidad en existensia ',
  `inventarioRenovable` tinyint NOT NULL COMMENT 'campo que dice si es parte del inventario fijio o que se renueva seguido',
  `fecha_ingreso` date NOT NULL COMMENT 'Fecha de ultimo ingreso',
  `fecha_vencimiento` date DEFAULT NULL COMMENT 'Fecha de vencimiento Proximo',
  PRIMARY KEY (`id`),
  KEY `fk_insumos_tipo_insumo1_idx` (`id_tipo_insumo`),
  KEY `fk_insumos_unidad_medida1_idx` (`id_unidad_medida`),
  CONSTRAINT `fk_insumos_tipo_insumo1` FOREIGN KEY (`id_tipo_insumo`) REFERENCES `tipo_insumo_utensilio` (`id`),
  CONSTRAINT `fk_insumos_unidad_medida1` FOREIGN KEY (`id_unidad_medida`) REFERENCES `unidad_medida` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `insumo_utensilio`
--

LOCK TABLES `insumo_utensilio` WRITE;
/*!40000 ALTER TABLE `insumo_utensilio` DISABLE KEYS */;
INSERT INTO `insumo_utensilio` VALUES (3,3,'Azucar',1,15,0,0,'2024-10-10','2024-12-20'),(4,3,'Vainilla',1,2.5,3,1,'2024-10-31','2024-10-07'),(5,3,'PPH',1,25,0,0,'2024-10-10','2024-12-20');
/*!40000 ALTER TABLE `insumo_utensilio` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `masas`
--

DROP TABLE IF EXISTS `masas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `masas` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'id de sabor de masa',
  `sabor_masa` varchar(250) NOT NULL COMMENT 'nombre del sabor de la masa',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `masas`
--

LOCK TABLES `masas` WRITE;
/*!40000 ALTER TABLE `masas` DISABLE KEYS */;
INSERT INTO `masas` VALUES (6,'Chocolate'),(7,'Vainilla'),(8,'Red Velvet'),(9,'naranja');
/*!40000 ALTER TABLE `masas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `modulo`
--

DROP TABLE IF EXISTS `modulo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `modulo` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'id de modulo',
  `nombre` varchar(100) NOT NULL COMMENT 'nombre del modulo',
  `url` varchar(100) NOT NULL COMMENT 'url que llevara a la pagina donde se encuentra la informacion del modulo',
  `icono` varchar(100) NOT NULL COMMENT 'icono del modulo para mostar en base de datos',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `modulo`
--

LOCK TABLES `modulo` WRITE;
/*!40000 ALTER TABLE `modulo` DISABLE KEYS */;
/*!40000 ALTER TABLE `modulo` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `motivo_salida`
--

DROP TABLE IF EXISTS `motivo_salida`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `motivo_salida` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'id de motivo de salida del inventario',
  `nombre` varchar(100) NOT NULL COMMENT 'nombre de motivo de salida del inventario',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `motivo_salida`
--

LOCK TABLES `motivo_salida` WRITE;
/*!40000 ALTER TABLE `motivo_salida` DISABLE KEYS */;
INSERT INTO `motivo_salida` VALUES (1,'vencido');
/*!40000 ALTER TABLE `motivo_salida` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `observacion_cotizacion_online`
--

DROP TABLE IF EXISTS `observacion_cotizacion_online`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `observacion_cotizacion_online` (
  `correlativo` int NOT NULL AUTO_INCREMENT,
  `id_cotizacion_online` int NOT NULL,
  `Observacion` varchar(255) NOT NULL,
  PRIMARY KEY (`correlativo`),
  KEY `fk_cotizacion_online_obs` (`id_cotizacion_online`),
  CONSTRAINT `fk_cotizacion_online_obs` FOREIGN KEY (`id_cotizacion_online`) REFERENCES `cotizacion_online` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 ;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `observacion_cotizacion_online`
--

LOCK TABLES `observacion_cotizacion_online` WRITE;
/*!40000 ALTER TABLE `observacion_cotizacion_online` DISABLE KEYS */;
/*!40000 ALTER TABLE `observacion_cotizacion_online` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pastel_realizado`
--

DROP TABLE IF EXISTS `pastel_realizado`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pastel_realizado` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'id de pastel realizado finalizado',
  `id_tipo_evento` int NOT NULL COMMENT 'id tipo de evento',
  `id_pedido` int NOT NULL COMMENT 'id del pedido',
  `imagen` varchar(500) NOT NULL COMMENT 'ruta de la imagen del pedido entregado',
  PRIMARY KEY (`id`),
  KEY `fk_pastel_realizado_tipo_evemto1_idx` (`id_tipo_evento`),
  KEY `fk_pastel_realizado_pedido1_idx` (`id_pedido`),
  CONSTRAINT `fk_pastel_realizado_pedido1` FOREIGN KEY (`id_pedido`) REFERENCES `pedido` (`id`),
  CONSTRAINT `fk_pastel_realizado_tipo_evemto1` FOREIGN KEY (`id_tipo_evento`) REFERENCES `tipo_evento` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pastel_realizado`
--

LOCK TABLES `pastel_realizado` WRITE;
/*!40000 ALTER TABLE `pastel_realizado` DISABLE KEYS */;
INSERT INTO `pastel_realizado` VALUES (3,3,3,'https://firebasestorage.googleapis.com/v0/b/subida-ab43b.appspot.com/o/images%2Fpastel%20ref2.jpg?alt=media&token=194e6dc9-62b0-4955-9415-7d1d02bf630b');
/*!40000 ALTER TABLE `pastel_realizado` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pedido`
--

DROP TABLE IF EXISTS `pedido`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pedido` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'Se almacena el id del pedido.\\n',
  `id_estado` int NOT NULL COMMENT 'id del estado del pedido.',
  `observaciones` varchar(150) DEFAULT NULL COMMENT 'Se almacenan observaciones para el pedido.',
  `cotizacion_online_id` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_pedido_estado1_idx` (`id_estado`),
  KEY `fk_pedido_cotizacion_online1_idx` (`cotizacion_online_id`),
  CONSTRAINT `fk_pedido_cotizacion_online1` FOREIGN KEY (`cotizacion_online_id`) REFERENCES `cotizacion_online` (`id`),
  CONSTRAINT `fk_pedido_estado1` FOREIGN KEY (`id_estado`) REFERENCES `estado` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pedido`
--

LOCK TABLES `pedido` WRITE;
/*!40000 ALTER TABLE `pedido` DISABLE KEYS */;
INSERT INTO `pedido` VALUES (3,10,'se puede hacer',12);
/*!40000 ALTER TABLE `pedido` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `producto`
--

DROP TABLE IF EXISTS `producto`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `producto` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'Se almacena el id del producto.',
  `nombre` varchar(255) NOT NULL COMMENT 'Se almacena el nombre del producto.',
  `descripcion` varchar(500) DEFAULT NULL COMMENT 'Se almacena la descripcion del producto.',
  `precio_online` double NOT NULL COMMENT 'Se almacena el precio online.',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `producto`
--

LOCK TABLES `producto` WRITE;
/*!40000 ALTER TABLE `producto` DISABLE KEYS */;
INSERT INTO `producto` VALUES (4,'Pastel','Pastel sin Relleno',25),(5,'Cupcake Relleno','con fondant',30);
/*!40000 ALTER TABLE `producto` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `proveedor`
--

DROP TABLE IF EXISTS `proveedor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `proveedor` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'Se almacena el id del proveedor.',
  `nombre` varchar(200) NOT NULL COMMENT 'Se almacena el nombre del proveedor.',
  `telefono` int NOT NULL COMMENT 'Se almacena el numero de telefono del proveedor.',
  `descripcion` varchar(200) NOT NULL COMMENT 'Se almacena la descripcion.',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `proveedor`
--

LOCK TABLES `proveedor` WRITE;
/*!40000 ALTER TABLE `proveedor` DISABLE KEYS */;
INSERT INTO `proveedor` VALUES (3,'Steff',12345678,'Harin'),(4,'La Casa del Pastel',45678,'Anaite');
/*!40000 ALTER TABLE `proveedor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `receta`
--

DROP TABLE IF EXISTS `receta`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `receta` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'Se almacena el id de receta.',
  `nombre` varchar(255) NOT NULL COMMENT 'Se almacena el nombre de la receta.',
  `descripcion` longtext NOT NULL COMMENT 'Se almacena la descripcion de la receta.',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `receta`
--

LOCK TABLES `receta` WRITE;
/*!40000 ALTER TABLE `receta` DISABLE KEYS */;
INSERT INTO `receta` VALUES (3,'receta','prueba de receta'),(4,'nueva receta','nueva receta}'),(5,'prueba','prueba');
/*!40000 ALTER TABLE `receta` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `relleno`
--

DROP TABLE IF EXISTS `relleno`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `relleno` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'Se almacena el id del relleno.',
  `sabor_relleno` varchar(45) DEFAULT NULL COMMENT 'Se almacena el sabor del relleno.\\n',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `relleno`
--

LOCK TABLES `relleno` WRITE;
/*!40000 ALTER TABLE `relleno` DISABLE KEYS */;
INSERT INTO `relleno` VALUES (4,'Cerezas'),(5,'Cajeta'),(6,'Nutella');
/*!40000 ALTER TABLE `relleno` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `rol`
--

DROP TABLE IF EXISTS `rol`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `rol` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rol`
--

LOCK TABLES `rol` WRITE;
/*!40000 ALTER TABLE `rol` DISABLE KEYS */;
/*!40000 ALTER TABLE `rol` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `salida_inventario`
--

DROP TABLE IF EXISTS `salida_inventario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `salida_inventario` (
  `correlativo` int NOT NULL AUTO_INCREMENT,
  `id_insumo_utensilio` int NOT NULL,
  `cantidad` int NOT NULL,
  `id_motivo_salida` int NOT NULL,
  `id_encabezado_salida` int NOT NULL,
  PRIMARY KEY (`correlativo`),
  KEY `fk_salida_inventario_insumo_utensilio1_idx` (`id_insumo_utensilio`),
  KEY `fk_salida_inventario_motivo_salida1_idx` (`id_motivo_salida`),
  KEY `fk_salida_idx` (`id_encabezado_salida`),
  CONSTRAINT `fk_salida` FOREIGN KEY (`id_encabezado_salida`) REFERENCES `encabezado_salida` (`id_salida`),
  CONSTRAINT `fk_salida_inventario_insumo_utensilio1` FOREIGN KEY (`id_insumo_utensilio`) REFERENCES `insumo_utensilio` (`id`),
  CONSTRAINT `fk_salida_inventario_motivo_salida1` FOREIGN KEY (`id_motivo_salida`) REFERENCES `motivo_salida` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `salida_inventario`
--

LOCK TABLES `salida_inventario` WRITE;
/*!40000 ALTER TABLE `salida_inventario` DISABLE KEYS */;
INSERT INTO `salida_inventario` VALUES (1,3,1,1,1),(2,3,2,1,1),(3,4,2,1,2),(4,4,2,1,2),(5,3,10,1,3),(6,3,10,1,4),(7,4,5,1,4),(8,5,10,1,5),(9,3,0,1,5),(10,4,2,1,5);
/*!40000 ALTER TABLE `salida_inventario` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tipo_evento`
--

DROP TABLE IF EXISTS `tipo_evento`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tipo_evento` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tipo_evento`
--

LOCK TABLES `tipo_evento` WRITE;
/*!40000 ALTER TABLE `tipo_evento` DISABLE KEYS */;
INSERT INTO `tipo_evento` VALUES (3,'Bodas');
/*!40000 ALTER TABLE `tipo_evento` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tipo_insumo_utensilio`
--

DROP TABLE IF EXISTS `tipo_insumo_utensilio`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tipo_insumo_utensilio` (
  `id` int NOT NULL AUTO_INCREMENT,
  `tipo` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tipo_insumo_utensilio`
--

LOCK TABLES `tipo_insumo_utensilio` WRITE;
/*!40000 ALTER TABLE `tipo_insumo_utensilio` DISABLE KEYS */;
INSERT INTO `tipo_insumo_utensilio` VALUES (3,'Perecederos'),(4,'Perecedero');
/*!40000 ALTER TABLE `tipo_insumo_utensilio` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `unidad_medida`
--

DROP TABLE IF EXISTS `unidad_medida`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `unidad_medida` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `unidad_medida`
--

LOCK TABLES `unidad_medida` WRITE;
/*!40000 ALTER TABLE `unidad_medida` DISABLE KEYS */;
INSERT INTO `unidad_medida` VALUES (1,'unidad');
/*!40000 ALTER TABLE `unidad_medida` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `unidad_medida_precio_sugerido`
--

DROP TABLE IF EXISTS `unidad_medida_precio_sugerido`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `unidad_medida_precio_sugerido` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `unidad_medida_precio_sugerido`
--

LOCK TABLES `unidad_medida_precio_sugerido` WRITE;
/*!40000 ALTER TABLE `unidad_medida_precio_sugerido` DISABLE KEYS */;
INSERT INTO `unidad_medida_precio_sugerido` VALUES (2,'media cucharada');
/*!40000 ALTER TABLE `unidad_medida_precio_sugerido` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuario`
--

DROP TABLE IF EXISTS `usuario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuario` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'Se almacena el id del usuario.',
  `nombre` varchar(100) NOT NULL COMMENT 'Se almacena el nombre del usuario.',
  `correo` varchar(100) NOT NULL COMMENT 'Se almacena el correo electronico del usuario.',
  `contrasenia` varchar(500) NOT NULL COMMENT 'Se almacena la contrasenia del usuario.',
  `visibilidad` tinyint DEFAULT NULL,
  `id_rol` int NOT NULL COMMENT 'Id del rol del usuario.',
  PRIMARY KEY (`id`),
  KEY `fk_usuario_rol1_idx` (`id_rol`),
  CONSTRAINT `fk_usuario_rol1` FOREIGN KEY (`id_rol`) REFERENCES `rol` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuario`
--

LOCK TABLES `usuario` WRITE;
/*!40000 ALTER TABLE `usuario` DISABLE KEYS */;
/*!40000 ALTER TABLE `usuario` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-10-29 21:28:27
