-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 11-07-2024 a las 16:48:09
-- Versión del servidor: 10.4.28-MariaDB
-- Versión de PHP: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `menugratis`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `menus`
--

CREATE TABLE `menus` (
  `ID_MENU` int(11) NOT NULL,
  `ID_RES` int(11) DEFAULT NULL,
  `NOMBRE` varchar(250) DEFAULT NULL,
  `FECHA_INICIO` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `menus`
--

INSERT INTO `menus` (`ID_MENU`, `ID_RES`, `NOMBRE`, `FECHA_INICIO`) VALUES
(2, 11, 'Burger', '2024-04-03 21:56:12'),
(3, 12, 'Pizza_menu', '2024-04-03 22:02:08'),
(4, 13, 'La Bendición_menu', '2024-04-15 12:10:52'),
(5, 14, 'asd_menu', '2024-04-29 12:31:50'),
(6, 15, 'a_menu', '2024-05-04 15:59:00'),
(7, 16, 'Tacos El Pastor_menu', '2024-05-04 16:28:05');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `platillos`
--

CREATE TABLE `platillos` (
  `ID_PLATILLO` int(11) NOT NULL,
  `ID_MENU` int(11) DEFAULT NULL,
  `NOMBRE` varchar(250) DEFAULT NULL,
  `DESCRIPCION` varchar(1000) DEFAULT NULL,
  `PRECIO` double DEFAULT NULL,
  `FECHA_CREADO` datetime DEFAULT NULL,
  `IMAGEN_PLATILLO` text DEFAULT NULL,
  `TIPO` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `platillos`
--

INSERT INTO `platillos` (`ID_PLATILLO`, `ID_MENU`, `NOMBRE`, `DESCRIPCION`, `PRECIO`, `FECHA_CREADO`, `IMAGEN_PLATILLO`, `TIPO`) VALUES
(59, 3, 'Pizza', 'Rica pizza', 55, '2024-05-04 16:22:44', '/Images/Platillos/pizza-con-chorizo-jamon-y-queso-1080x671_80cf3896-e7da-4d9a-9ae3-4e0e026de8d3.jpg', 'Piz');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `restaurantes`
--

CREATE TABLE `restaurantes` (
  `ID` int(11) NOT NULL,
  `NOMBRE` varchar(100) DEFAULT NULL,
  `APELLIDO` varchar(100) DEFAULT NULL,
  `TELEFONO` varchar(100) DEFAULT NULL,
  `SEXO` varchar(15) DEFAULT NULL,
  `CORREO` varchar(150) NOT NULL,
  `CONTRASENA` varchar(150) NOT NULL,
  `RESTAURANTE` varchar(200) DEFAULT NULL,
  `UBICACION_RESTAURANTE` int(11) DEFAULT NULL,
  `RESTABLECER` tinyint(1) NOT NULL DEFAULT 0,
  `FEC_REGISTRO` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `restaurantes`
--

INSERT INTO `restaurantes` (`ID`, `NOMBRE`, `APELLIDO`, `TELEFONO`, `SEXO`, `CORREO`, `CONTRASENA`, `RESTAURANTE`, `UBICACION_RESTAURANTE`, `RESTABLECER`, `FEC_REGISTRO`) VALUES
(11, 'Deyvis Saul', 'Benito Medrano', '00000000', 'Masculino', 'deyvisbenito@gmail.com', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'Burger', 11, 0, '2024-04-03'),
(12, 'Me', 'Benito Medrano', '1234523', 'Masculino', 'test@gmail.com', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'Sereal', 12, 0, '2024-04-03'),
(13, 'Antonio', 'Benito', '46172036', 'Masculino', 'antonio@gmail.com', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'La Bendición', 13, 0, '2024-04-15'),
(14, 'asd', 'asd', '123', 'Masculino', 'asd@asd', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'asd', 14, 0, '2024-04-29'),
(15, 'dey', 'a', '123', 'Femenino', 'test1@gmail.com', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'a', 15, 0, '2024-05-04'),
(16, 'Luis ', 'Pelico', '123456', 'Masculino', 'pelico@gmail.com', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'Tacos El Pastor', 16, 0, '2024-05-04');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `secciones`
--

CREATE TABLE `secciones` (
  `ID_SE` int(11) NOT NULL,
  `NOMBRE` varchar(100) DEFAULT NULL,
  `ID_RES` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `secciones`
--

INSERT INTO `secciones` (`ID_SE`, `NOMBRE`, `ID_RES`) VALUES
(8, 'aaaa', 14),
(43, 'Piz', 12);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `ubicaciones`
--

CREATE TABLE `ubicaciones` (
  `ID_RES` int(11) NOT NULL,
  `PAIS` varchar(100) DEFAULT NULL,
  `DEPARTAMENTO` varchar(200) DEFAULT NULL,
  `MUNICIPIO` varchar(200) DEFAULT NULL,
  `ZONA` varchar(100) DEFAULT NULL,
  `CALLE` varchar(200) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `ubicaciones`
--

INSERT INTO `ubicaciones` (`ID_RES`, `PAIS`, `DEPARTAMENTO`, `MUNICIPIO`, `ZONA`, `CALLE`) VALUES
(11, 'Guatemala', 'Suchi', 'Null', 'Linea B6', 'NULL'),
(12, 'PEPEP', 'Suchi', 'Null', 'Parque', 'NULL'),
(13, 'Guatemala', 'Suchi', 'Maquina', '5', 'B6'),
(14, 'aaaa', 'bbbb', 'cccc', 'ddddd', 'eeee'),
(15, 'aaaa', 'bbbb', 'cccc', 'ddddd', 'eeee'),
(16, 'Guatemala', 'Suchitepequez', 'Mazatenango', 'Zona 4', '4-5');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `menus`
--
ALTER TABLE `menus`
  ADD PRIMARY KEY (`ID_MENU`),
  ADD KEY `ID_RES` (`ID_RES`);

--
-- Indices de la tabla `platillos`
--
ALTER TABLE `platillos`
  ADD PRIMARY KEY (`ID_PLATILLO`),
  ADD KEY `ID_MENU` (`ID_MENU`);

--
-- Indices de la tabla `restaurantes`
--
ALTER TABLE `restaurantes`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `CORREO` (`CORREO`),
  ADD KEY `FK_UBICACION_RESTAURANTE` (`UBICACION_RESTAURANTE`);

--
-- Indices de la tabla `secciones`
--
ALTER TABLE `secciones`
  ADD PRIMARY KEY (`ID_SE`),
  ADD KEY `ID_RES` (`ID_RES`);

--
-- Indices de la tabla `ubicaciones`
--
ALTER TABLE `ubicaciones`
  ADD PRIMARY KEY (`ID_RES`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `menus`
--
ALTER TABLE `menus`
  MODIFY `ID_MENU` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `platillos`
--
ALTER TABLE `platillos`
  MODIFY `ID_PLATILLO` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=60;

--
-- AUTO_INCREMENT de la tabla `restaurantes`
--
ALTER TABLE `restaurantes`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT de la tabla `secciones`
--
ALTER TABLE `secciones`
  MODIFY `ID_SE` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=45;

--
-- AUTO_INCREMENT de la tabla `ubicaciones`
--
ALTER TABLE `ubicaciones`
  MODIFY `ID_RES` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `menus`
--
ALTER TABLE `menus`
  ADD CONSTRAINT `menus_ibfk_1` FOREIGN KEY (`ID_RES`) REFERENCES `restaurantes` (`ID`);

--
-- Filtros para la tabla `platillos`
--
ALTER TABLE `platillos`
  ADD CONSTRAINT `platillos_ibfk_1` FOREIGN KEY (`ID_MENU`) REFERENCES `menus` (`ID_MENU`);

--
-- Filtros para la tabla `restaurantes`
--
ALTER TABLE `restaurantes`
  ADD CONSTRAINT `FK_UBICACION_RESTAURANTE` FOREIGN KEY (`UBICACION_RESTAURANTE`) REFERENCES `ubicaciones` (`ID_RES`);

--
-- Filtros para la tabla `secciones`
--
ALTER TABLE `secciones`
  ADD CONSTRAINT `secciones_ibfk_1` FOREIGN KEY (`ID_RES`) REFERENCES `restaurantes` (`ID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
