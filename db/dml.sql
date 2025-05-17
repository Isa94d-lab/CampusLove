DROP DATABASE CampusLove;
CREATE DATABASE IF NOT EXISTS CampusLove;

USE CampusLove;

CREATE TABLE IF NOT EXISTS Genero (
    id INT PRIMARY KEY AUTO_INCREMENT,
    descripcion VARCHAR(25)
);

CREATE TABLE IF NOT EXISTS Profesion (
    id INT PRIMARY KEY AUTO_INCREMENT,
    descripcion VARCHAR(30)
);

CREATE TABLE IF NOT EXISTS Intereses (
    id INT PRIMARY KEY AUTO_INCREMENT,
    tipo VARCHAR(25)
);

CREATE TABLE IF NOT EXISTS EstadoPerfil (
    id INT PRIMARY KEY AUTO_INCREMENT,
    descripcion VARCHAR(10)
);

CREATE TABLE IF NOT EXISTS Perfil (
    id INT PRIMARY KEY AUTO_INCREMENT,
    profesion_id INT,
    genero_id INT,
    estadoPerfil_id INT,
    nombre VARCHAR(20),
    apellido VARCHAR(20),
    edad INT,
    frase VARCHAR(40),
    gustos VARCHAR(40),
    coins INT,
    CONSTRAINT profesion_perfil_FK FOREIGN KEY (profesion_id) REFERENCES Profesion(id),
    CONSTRAINT genero_perfil_FK FOREIGN KEY (genero_id) REFERENCES Genero(id),
    CONSTRAINT estadoPerfil_perfil_FK FOREIGN KEY (estadoPerfil_id) REFERENCES EstadoPerfil(id)
);

CREATE TABLE IF NOT EXISTS PerfilIntereses (
    id INT PRIMARY KEY AUTO_INCREMENT,
    perfil_id INT,
    intereses_id INT,
    CONSTRAINT perfil_perfilIntereses_FK FOREIGN KEY (perfil_id) REFERENCES Perfil(id),
    CONSTRAINT intereses_perfilIntereses_FK FOREIGN KEY (intereses_id) REFERENCES Intereses(id)
);

CREATE TABLE IF NOT EXISTS Usuario (
    id INT PRIMARY KEY AUTO_INCREMENT, 
    perfil_id INT,
    nickname VARCHAR(20),
    password VARCHAR(20),
    CONSTRAINT perfil_usuario_FK FOREIGN KEY (perfil_id) REFERENCES Perfil(id) 
);

CREATE TABLE IF NOT EXISTS Matchs (
    id INT PRIMARY KEY AUTO_INCREMENT,
    perfil1_id INT,
    perfil2_id INT,
    fecha DATE,
    CONSTRAINT perfil1_match_FK FOREIGN KEY (perfil1_id) REFERENCES Perfil(id),
    CONSTRAINT perfil2_match_FK FOREIGN KEY (perfil2_id) REFERENCES Perfil(id)
);

CREATE TABLE IF NOT EXISTS Interaccion (
    id INT PRIMARY KEY AUTO_INCREMENT, 
    usuario_id INT,
    perfil_id INT,
    reaccion ENUM ('Like', 'Dislike'),
    fecha DATE,
    CONSTRAINT usuario_interaccion_FK FOREIGN KEY (usuario_id) REFERENCES Usuario(id),
    CONSTRAINT perfil_interaccion_FK FOREIGN KEY (perfil_id) REFERENCES Perfil(id) 
);

CREATE TABLE IF NOT EXISTS LikesDiarios (
    id INT PRIMARY KEY AUTO_INCREMENT,
    perfil_id INT,
    cantidad INT,
    CONSTRAINT perfil_likesDiarios_FK FOREIGN KEY (perfil_id) REFERENCES Perfil(id) ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS LikesUsuario (
    id INT PRIMARY KEY AUTO_INCREMENT,
    usuario_id INT,
    perfil_likeado_id INT,
    fechaLike DATE,
    match_r BOOLEAN DEFAULT FALSE,
    CONSTRAINT usuario_likesUsuario_FK FOREIGN KEY (usuario_id) REFERENCES Usuario(id),
    CONSTRAINT perfil_likeado_FK FOREIGN KEY (perfil_likeado_id) REFERENCES Perfil(id),
    UNIQUE KEY unique_like (usuario_id, perfil_likeado_id)
);