-- tables
-- Table: Candidatos
CREATE TABLE Candidatos (
    candidato_id int  NOT NULL IDENTITY,
    candidato_fec_inscripcion datetime  NOT NULL,
    usuario_id int  NOT NULL,
    CONSTRAINT Candidatos_pk PRIMARY KEY  (candidato_id)
);

-- Table: Cursos
CREATE TABLE Cursos (
    curso_id int  NOT NULL IDENTITY,
    curso_nombre varchar(50)  NOT NULL,
    curso_valido_votacion bit  NOT NULL,
    CONSTRAINT Cursos_pk PRIMARY KEY  (curso_id)
);

-- Table: Propuestas
CREATE TABLE Propuestas (
    propuesta_id int  NOT NULL IDENTITY,
    propuesta_nombre varchar(50)  NOT NULL,
    propuesta_descripcion varchar(500)  NOT NULL,
    candidato_id int  NOT NULL,
    CONSTRAINT Propuestas_pk PRIMARY KEY  (propuesta_id)
);

-- Table: Roles
CREATE TABLE Roles (
    rol_id int  NOT NULL IDENTITY,
    rol_descripcion varchar(50)  NOT NULL,
    CONSTRAINT Roles_pk PRIMARY KEY  (rol_id)
);

-- Table: Usuarios
CREATE TABLE Usuarios (
    usuario_id int  NOT NULL IDENTITY,
    usuario_nombres varchar(50)  NOT NULL,
    usuario_apellidos varchar(50)  NOT NULL,
    usuario_documento varchar(20)  NOT NULL,
    usuario_fec_nac date  NOT NULL,
    usuario_email varchar(100)  NOT NULL,
    usuario_password varchar(128)  NOT NULL,
    usuario_activo bit  NOT NULL,
    rol_id int  NOT NULL,
    curso_id int  NOT NULL,
    CONSTRAINT Usuarios_pk PRIMARY KEY  (usuario_id)
);

-- Table: Votos
CREATE TABLE Votos (
    voto_id int  NOT NULL IDENTITY,
    candidato_id int  NOT NULL,
    usuario_id int  NOT NULL,
    fecha_votacion datetime  NOT NULL,
    CONSTRAINT Votos_pk PRIMARY KEY  (voto_id)
);

-- foreign keys
-- Reference: Candidatos_Usuarios (table: Candidatos)
ALTER TABLE Candidatos ADD CONSTRAINT Candidatos_Usuarios
    FOREIGN KEY (usuario_id)
    REFERENCES Usuarios (usuario_id);

-- Reference: Propuestas_Candidatos (table: Propuestas)
ALTER TABLE Propuestas ADD CONSTRAINT Propuestas_Candidatos
    FOREIGN KEY (candidato_id)
    REFERENCES Candidatos (candidato_id);

-- Reference: Usuarios_Cursos (table: Usuarios)
ALTER TABLE Usuarios ADD CONSTRAINT Usuarios_Cursos
    FOREIGN KEY (curso_id)
    REFERENCES Cursos (curso_id);

-- Reference: Usuarios_Roles (table: Usuarios)
ALTER TABLE Usuarios ADD CONSTRAINT Usuarios_Roles
    FOREIGN KEY (rol_id)
    REFERENCES Roles (rol_id);

-- Reference: Votos_Candidatos (table: Votos)
ALTER TABLE Votos ADD CONSTRAINT Votos_Candidatos
    FOREIGN KEY (candidato_id)
    REFERENCES Candidatos (candidato_id);

-- Reference: Votos_Usuarios (table: Votos)
ALTER TABLE Votos ADD CONSTRAINT Votos_Usuarios
    FOREIGN KEY (usuario_id)
    REFERENCES Usuarios (usuario_id);