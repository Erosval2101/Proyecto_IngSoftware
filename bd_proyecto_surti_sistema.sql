-- phpMyAdmin SQL Dump
-- version 4.8.5
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1:3306
-- Tiempo de generación: 28-10-2019 a las 22:38:30
-- Versión del servidor: 5.7.26
-- Versión de PHP: 7.2.18

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `surtisistema`
--
CREATE DATABASE IF NOT EXISTS `surtisistema` DEFAULT CHARACTER SET latin1 COLLATE latin1_spanish_ci;
USE `surtisistema`;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `cliente`
--

DROP TABLE IF EXISTS `cliente`;
CREATE TABLE IF NOT EXISTS `cliente` (
  `IDCliente` int(11) NOT NULL,
  `NombreCliente` varchar(50) COLLATE latin1_spanish_ci NOT NULL,
  `Ap_PaternoCliente` varchar(30) COLLATE latin1_spanish_ci NOT NULL,
  `Ap_MaternoCliente` varchar(30) COLLATE latin1_spanish_ci NOT NULL,
  `Numero_Telefono` varchar(10) COLLATE latin1_spanish_ci NOT NULL,
  `CorreoElectronico` text COLLATE latin1_spanish_ci NOT NULL,
  `CreditoDisponible` float NOT NULL,
  PRIMARY KEY (`IDCliente`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `compras`
--

DROP TABLE IF EXISTS `compras`;
CREATE TABLE IF NOT EXISTS `compras` (
  `IDCompra` int(11) NOT NULL,
  `ID_Empleado` int(11) NOT NULL,
  `ID_Proveedor` int(11) NOT NULL,
  `FechaInicio` datetime NOT NULL,
  `FechaCierre` datetime NOT NULL,
  `Descripcion` text COLLATE latin1_spanish_ci NOT NULL,
  `Estatus` enum('En proceso','Finalizada','Cancelada','') COLLATE latin1_spanish_ci NOT NULL,
  PRIMARY KEY (`IDCompra`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `departamento`
--

DROP TABLE IF EXISTS `departamento`;
CREATE TABLE IF NOT EXISTS `departamento` (
  `IDDepartamento` int(11) NOT NULL,
  `Nombre_Dep` varchar(30) COLLATE latin1_spanish_ci NOT NULL,
  `IDEmpleado` int(11) NOT NULL,
  PRIMARY KEY (`IDDepartamento`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `descripcion_producto`
--

DROP TABLE IF EXISTS `descripcion_producto`;
CREATE TABLE IF NOT EXISTS `descripcion_producto` (
  `IDproducto` int(11) NOT NULL,
  `No_Producto` int(11) NOT NULL,
  `Color` varchar(30) COLLATE latin1_spanish_ci NOT NULL,
  `Modelo` varchar(30) COLLATE latin1_spanish_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `detalle_compra`
--

DROP TABLE IF EXISTS `detalle_compra`;
CREATE TABLE IF NOT EXISTS `detalle_compra` (
  `ID_Compra` int(11) NOT NULL,
  `ID_Producto` int(11) NOT NULL,
  `CantidadProductos` int(11) NOT NULL,
  `PrecioProducto` float NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `detalle_de_devolucion`
--

DROP TABLE IF EXISTS `detalle_de_devolucion`;
CREATE TABLE IF NOT EXISTS `detalle_de_devolucion` (
  `ID_Devolucion` int(11) NOT NULL,
  `ID_Producto` int(11) NOT NULL,
  `Cantidad_Productos` int(11) NOT NULL,
  `Motivo_Devolucion` text COLLATE latin1_spanish_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `detalle_venta`
--

DROP TABLE IF EXISTS `detalle_venta`;
CREATE TABLE IF NOT EXISTS `detalle_venta` (
  `ID_Venta` int(11) NOT NULL,
  `Cantidad` int(11) NOT NULL,
  `Precio` float NOT NULL,
  `ID_Producto` int(11) NOT NULL,
  `MontoPagar` float NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `devoluciones`
--

DROP TABLE IF EXISTS `devoluciones`;
CREATE TABLE IF NOT EXISTS `devoluciones` (
  `IDDevolucion` int(11) NOT NULL,
  `ID_Venta` int(11) NOT NULL,
  `FechaDevolucion` datetime NOT NULL,
  PRIMARY KEY (`IDDevolucion`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `empleado`
--

DROP TABLE IF EXISTS `empleado`;
CREATE TABLE IF NOT EXISTS `empleado` (
  `IDEmpleado` int(11) NOT NULL,
  `Nombre_Empleado` varchar(50) COLLATE latin1_spanish_ci NOT NULL,
  `Ap_Paterno` varchar(30) COLLATE latin1_spanish_ci NOT NULL,
  `Ap_Materno` varchar(30) COLLATE latin1_spanish_ci NOT NULL,
  `Puesto` varchar(30) COLLATE latin1_spanish_ci NOT NULL,
  `H_Entrada` time NOT NULL,
  `H_Salida` time NOT NULL,
  `Numero_Telefono` varchar(10) COLLATE latin1_spanish_ci NOT NULL,
  `CorreoElectronico` text COLLATE latin1_spanish_ci NOT NULL,
  `Contrasena` int(11) NOT NULL,
  `Salario` int(11) NOT NULL,
  `IDDepartamento` int(11) NOT NULL,
  PRIMARY KEY (`IDEmpleado`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago_con_credito`
--

DROP TABLE IF EXISTS `pago_con_credito`;
CREATE TABLE IF NOT EXISTS `pago_con_credito` (
  `IDCliente` int(11) NOT NULL,
  `Monto_a_pagar` float NOT NULL,
  `FechaDePago` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `producto`
--

DROP TABLE IF EXISTS `producto`;
CREATE TABLE IF NOT EXISTS `producto` (
  `IDProducto` int(11) NOT NULL,
  `Descripcion` text COLLATE latin1_spanish_ci NOT NULL,
  `Precio` float NOT NULL,
  `Disponible` tinyint(1) NOT NULL,
  `Existencias` int(11) NOT NULL,
  `IDDepartamento` int(11) NOT NULL,
  PRIMARY KEY (`IDProducto`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `producto_del_proveedor`
--

DROP TABLE IF EXISTS `producto_del_proveedor`;
CREATE TABLE IF NOT EXISTS `producto_del_proveedor` (
  `IDProducto` int(11) NOT NULL,
  `ID_Proveedor` int(11) NOT NULL,
  `Disponible` tinyint(1) NOT NULL,
  `Precio` float NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `proveedores`
--

DROP TABLE IF EXISTS `proveedores`;
CREATE TABLE IF NOT EXISTS `proveedores` (
  `IDProveedor` int(11) NOT NULL,
  `Nombre_Prov` varchar(40) COLLATE latin1_spanish_ci NOT NULL,
  `Direccion` text COLLATE latin1_spanish_ci NOT NULL,
  `Numero_Telefono` varchar(10) COLLATE latin1_spanish_ci NOT NULL,
  `CorreoElectronico` int(11) NOT NULL,
  PRIMARY KEY (`IDProveedor`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `venta`
--

DROP TABLE IF EXISTS `venta`;
CREATE TABLE IF NOT EXISTS `venta` (
  `IDVenta` int(11) NOT NULL,
  `ID_Empleado` int(11) NOT NULL,
  `ID_Cliente` int(11) NOT NULL,
  `FechaVenta` datetime NOT NULL,
  `TipoPago` enum('Efectivo','Tarjeta Bancaria','Crédito Tienda','') COLLATE latin1_spanish_ci NOT NULL,
  PRIMARY KEY (`IDVenta`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
