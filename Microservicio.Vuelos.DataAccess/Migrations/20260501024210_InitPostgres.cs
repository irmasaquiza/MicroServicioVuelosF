using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Microservicio.Vuelos.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "aero");

            migrationBuilder.EnsureSchema(
                name: "vuelos");

            migrationBuilder.EnsureSchema(
                name: "crm");

            migrationBuilder.EnsureSchema(
                name: "ventas");

            migrationBuilder.EnsureSchema(
                name: "seg");

            migrationBuilder.CreateTable(
                name: "AUDITORIA_LOG",
                schema: "crm",
                columns: table => new
                {
                    id_auditoria = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    auditoria_guid = table.Column<Guid>(type: "uuid", nullable: false),
                    tabla_afectada = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    operacion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    id_registro_afectado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    datos_anteriores = table.Column<string>(type: "text", nullable: true),
                    datos_nuevos = table.Column<string>(type: "text", nullable: true),
                    usuario_ejecutor = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ip_origen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    fecha_evento_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false, defaultValueSql: "decode('00000001','hex')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUDITORIA_LOG", x => x.id_auditoria);
                });

            migrationBuilder.CreateTable(
                name: "Pais",
                schema: "aero",
                columns: table => new
                {
                    id_pais = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo_iso2 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    codigo_iso3 = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    continente = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pais", x => x.id_pais);
                });

            migrationBuilder.CreateTable(
                name: "ROL",
                schema: "seg",
                columns: table => new
                {
                    id_rol = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rol_guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    nombre_rol = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    descripcion_rol = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    estado_rol = table.Column<string>(type: "char(3)", nullable: false, defaultValue: "ACT"),
                    es_eliminado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "SYSTEM"),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false, defaultValueSql: "decode('00000001','hex')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROL", x => x.id_rol);
                });

            migrationBuilder.CreateTable(
                name: "CIUDAD",
                schema: "aero",
                columns: table => new
                {
                    id_ciudad = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false, defaultValueSql: "decode('00000001','hex')"),
                    id_pais = table.Column<int>(type: "integer", nullable: false),
                    nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    codigo_postal = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    zona_horaria = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    latitud = table.Column<decimal>(type: "numeric(9,6)", nullable: true),
                    longitud = table.Column<decimal>(type: "numeric(9,6)", nullable: true),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modificacion_ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CIUDAD", x => x.id_ciudad);
                    table.ForeignKey(
                        name: "FK_CIUDAD_Pais_id_pais",
                        column: x => x.id_pais,
                        principalSchema: "aero",
                        principalTable: "Pais",
                        principalColumn: "id_pais",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AEROPUERTO",
                schema: "aero",
                columns: table => new
                {
                    id_aeropuerto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false, defaultValueSql: "decode('00000001','hex')"),
                    codigo_iata = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    codigo_icao = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    id_ciudad = table.Column<int>(type: "integer", nullable: false),
                    id_pais = table.Column<int>(type: "integer", nullable: false),
                    zona_horaria = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    latitud = table.Column<decimal>(type: "numeric(9,6)", nullable: true),
                    longitud = table.Column<decimal>(type: "numeric(9,6)", nullable: true),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modificacion_ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AEROPUERTO", x => x.id_aeropuerto);
                    table.ForeignKey(
                        name: "FK_AEROPUERTO_CIUDAD_id_ciudad",
                        column: x => x.id_ciudad,
                        principalSchema: "aero",
                        principalTable: "CIUDAD",
                        principalColumn: "id_ciudad",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AEROPUERTO_Pais_id_pais",
                        column: x => x.id_pais,
                        principalSchema: "aero",
                        principalTable: "Pais",
                        principalColumn: "id_pais",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CLIENTES",
                schema: "crm",
                columns: table => new
                {
                    id_cliente = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cliente_guid = table.Column<Guid>(type: "uuid", nullable: false),
                    tipo_identificacion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    numero_identificacion = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    nombres = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    apellidos = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    razon_social = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    correo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    direccion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    id_ciudad_residencia = table.Column<int>(type: "integer", nullable: false),
                    id_pais_nacionalidad = table.Column<int>(type: "integer", nullable: false),
                    fecha_nacimiento = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    nacionalidad = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    genero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    es_eliminado = table.Column<bool>(type: "boolean", nullable: false),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modificacion_ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    servicio_origen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    fecha_inhabilitacion_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    motivo_inhabilitacion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false, defaultValueSql: "decode('00000001','hex')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLIENTES", x => x.id_cliente);
                    table.ForeignKey(
                        name: "FK_CLIENTES_CIUDAD_id_ciudad_residencia",
                        column: x => x.id_ciudad_residencia,
                        principalSchema: "aero",
                        principalTable: "CIUDAD",
                        principalColumn: "id_ciudad",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CLIENTES_Pais_id_pais_nacionalidad",
                        column: x => x.id_pais_nacionalidad,
                        principalSchema: "aero",
                        principalTable: "Pais",
                        principalColumn: "id_pais",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vuelo",
                schema: "vuelos",
                columns: table => new
                {
                    id_vuelo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false, defaultValueSql: "decode('00000001','hex')"),
                    numero_vuelo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    id_aeropuerto_origen = table.Column<int>(type: "integer", nullable: false),
                    id_aeropuerto_destino = table.Column<int>(type: "integer", nullable: false),
                    fecha_hora_salida = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fecha_hora_llegada = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    duracion_min = table.Column<int>(type: "integer", nullable: false),
                    precio_base = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    capacidad_total = table.Column<int>(type: "integer", nullable: false),
                    estado_vuelo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modificacion_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vuelo", x => x.id_vuelo);
                    table.ForeignKey(
                        name: "FK_Vuelo_AEROPUERTO_id_aeropuerto_destino",
                        column: x => x.id_aeropuerto_destino,
                        principalSchema: "aero",
                        principalTable: "AEROPUERTO",
                        principalColumn: "id_aeropuerto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vuelo_AEROPUERTO_id_aeropuerto_origen",
                        column: x => x.id_aeropuerto_origen,
                        principalSchema: "aero",
                        principalTable: "AEROPUERTO",
                        principalColumn: "id_aeropuerto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pasajero",
                schema: "ventas",
                columns: table => new
                {
                    id_pasajero = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false, defaultValueSql: "decode('00000001','hex')"),
                    id_cliente = table.Column<int>(type: "integer", nullable: true),
                    nombre_pasajero = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    apellido_pasajero = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    tipo_documento_pasajero = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    numero_documento_pasajero = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    fecha_nacimiento_pasajero = table.Column<DateTime>(type: "date", nullable: true),
                    nacionalidad_pasajero = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    email_contacto_pasajero = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    telefono_contacto_pasajero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    genero_pasajero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    requiere_asistencia = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    observaciones_pasajero = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "ACTIVO"),
                    es_eliminado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modificado_por_usuario = table.Column<string>(type: "text", nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modificacion_ip = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pasajero", x => x.id_pasajero);
                    table.ForeignKey(
                        name: "FK_Pasajero_Cliente",
                        column: x => x.id_cliente,
                        principalSchema: "crm",
                        principalTable: "CLIENTES",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "USUARIO_APP",
                schema: "seg",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    usuario_guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    id_cliente = table.Column<int>(type: "integer", nullable: true),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    correo = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    password_salt = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    fecha_ultimo_login = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    estado_usuario = table.Column<string>(type: "char(3)", nullable: false, defaultValue: "ACT"),
                    es_eliminado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "SYSTEM"),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modificacion_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false, defaultValueSql: "decode('00000001','hex')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO_APP", x => x.id_usuario);
                    table.ForeignKey(
                        name: "FK_USUARIO_APP_CLIENTE",
                        column: x => x.id_cliente,
                        principalSchema: "crm",
                        principalTable: "CLIENTES",
                        principalColumn: "id_cliente");
                });

            migrationBuilder.CreateTable(
                name: "ASIENTO",
                schema: "vuelos",
                columns: table => new
                {
                    id_asiento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false, defaultValueSql: "decode('00000001','hex')"),
                    id_vuelo = table.Column<int>(type: "integer", nullable: false),
                    numero_asiento = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    clase = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    disponible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    precio_extra = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    posicion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modificacion_ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASIENTO", x => x.id_asiento);
                    table.ForeignKey(
                        name: "FK_ASIENTO_Vuelo_id_vuelo",
                        column: x => x.id_vuelo,
                        principalSchema: "vuelos",
                        principalTable: "Vuelo",
                        principalColumn: "id_vuelo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Escala",
                schema: "vuelos",
                columns: table => new
                {
                    id_escala = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false, defaultValueSql: "decode('00000001','hex')"),
                    id_vuelo = table.Column<int>(type: "integer", nullable: false),
                    id_aeropuerto = table.Column<int>(type: "integer", nullable: false),
                    orden = table.Column<int>(type: "integer", nullable: false),
                    fecha_hora_llegada = table.Column<DateTime>(type: "timestamp", nullable: false),
                    fecha_hora_salida = table.Column<DateTime>(type: "timestamp", nullable: false),
                    duracion_min = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    tipo_escala = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "COMERCIAL"),
                    terminal = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    puerta = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    observaciones = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "ACTIVO"),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "SYSTEM"),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp", nullable: true),
                    modificacion_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Escala", x => x.id_escala);
                    table.CheckConstraint("CK_Escala_Duracion", "duracion_min >= 0");
                    table.CheckConstraint("CK_Escala_Fechas", "fecha_hora_salida > fecha_hora_llegada");
                    table.CheckConstraint("CK_Escala_Orden", "orden >= 1");
                    table.CheckConstraint("CK_Escala_Tipo", "tipo_escala IN ('TECNICA','COMERCIAL')");
                    table.ForeignKey(
                        name: "FK_Escala_AEROPUERTO_id_aeropuerto",
                        column: x => x.id_aeropuerto,
                        principalSchema: "aero",
                        principalTable: "AEROPUERTO",
                        principalColumn: "id_aeropuerto",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Escala_Vuelo_id_vuelo",
                        column: x => x.id_vuelo,
                        principalSchema: "vuelos",
                        principalTable: "Vuelo",
                        principalColumn: "id_vuelo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USUARIOS_ROLES",
                schema: "seg",
                columns: table => new
                {
                    id_usuario_rol = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_usuario = table.Column<int>(type: "integer", nullable: false),
                    id_rol = table.Column<int>(type: "integer", nullable: false),
                    estado_usuario_rol = table.Column<string>(type: "char(3)", nullable: false, defaultValue: "ACT"),
                    es_eliminado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "SYSTEM"),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false, defaultValueSql: "decode('00000001','hex')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIOS_ROLES", x => x.id_usuario_rol);
                    table.ForeignKey(
                        name: "FK_USUARIOS_ROLES_ROL",
                        column: x => x.id_rol,
                        principalSchema: "seg",
                        principalTable: "ROL",
                        principalColumn: "id_rol",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_USUARIOS_ROLES_USUARIO",
                        column: x => x.id_usuario,
                        principalSchema: "seg",
                        principalTable: "USUARIO_APP",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RESERVAS",
                schema: "ventas",
                columns: table => new
                {
                    id_reserva = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid_reserva = table.Column<Guid>(type: "uuid", nullable: false),
                    codigo_reserva = table.Column<string>(type: "text", nullable: false),
                    id_cliente = table.Column<int>(type: "integer", nullable: false),
                    id_pasajero = table.Column<int>(type: "integer", nullable: false),
                    id_vuelo = table.Column<int>(type: "integer", nullable: false),
                    id_asiento = table.Column<int>(type: "integer", nullable: false),
                    fecha_reserva_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fecha_inicio = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fecha_fin = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fecha_confirmacion_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    fecha_cancelacion_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    subtotal_reserva = table.Column<decimal>(type: "numeric", nullable: false),
                    valor_iva = table.Column<decimal>(type: "numeric", nullable: false),
                    total_reserva = table.Column<decimal>(type: "numeric", nullable: false),
                    estado_reserva = table.Column<string>(type: "text", nullable: false),
                    origen_canal_reserva = table.Column<string>(type: "text", nullable: false),
                    motivo_cancelacion = table.Column<string>(type: "text", nullable: true),
                    contacto_email = table.Column<string>(type: "text", nullable: true),
                    contacto_telefono = table.Column<string>(type: "text", nullable: true),
                    observaciones = table.Column<string>(type: "text", nullable: true),
                    servicio_origen = table.Column<string>(type: "text", nullable: false),
                    fecha_inhabilitacion_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    motivo_inhabilitacion = table.Column<string>(type: "text", nullable: true),
                    es_eliminado = table.Column<bool>(type: "boolean", nullable: false),
                    creado_por_usuario = table.Column<string>(type: "text", nullable: false),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modificado_por_usuario = table.Column<string>(type: "text", nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modificacion_ip = table.Column<string>(type: "text", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false, defaultValueSql: "decode('00000001','hex')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RESERVAS", x => x.id_reserva);
                    table.ForeignKey(
                        name: "FK_RESERVAS_ASIENTO_id_asiento",
                        column: x => x.id_asiento,
                        principalSchema: "vuelos",
                        principalTable: "ASIENTO",
                        principalColumn: "id_asiento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RESERVAS_CLIENTES_id_cliente",
                        column: x => x.id_cliente,
                        principalSchema: "crm",
                        principalTable: "CLIENTES",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RESERVAS_Pasajero_id_pasajero",
                        column: x => x.id_pasajero,
                        principalSchema: "ventas",
                        principalTable: "Pasajero",
                        principalColumn: "id_pasajero",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RESERVAS_Vuelo_id_vuelo",
                        column: x => x.id_vuelo,
                        principalSchema: "vuelos",
                        principalTable: "Vuelo",
                        principalColumn: "id_vuelo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Facturas",
                schema: "ventas",
                columns: table => new
                {
                    id_factura = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid_factura = table.Column<Guid>(type: "uuid", nullable: false),
                    id_cliente = table.Column<int>(type: "integer", nullable: false),
                    id_reserva = table.Column<int>(type: "integer", nullable: false),
                    numero_factura = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    fecha_emision = table.Column<DateTime>(type: "timestamp", nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    valor_iva = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    cargo_servicio = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    total = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    observaciones_factura = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    origen_canal_factura = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    estado = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    fecha_inhabilitacion_utc = table.Column<DateTime>(type: "timestamp", nullable: true),
                    es_eliminado = table.Column<bool>(type: "boolean", nullable: false),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp", nullable: false),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp", nullable: true),
                    modificacion_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    servicio_origen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    motivo_inhabilitacion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false, defaultValueSql: "decode('00000001','hex')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturas", x => x.id_factura);
                    table.ForeignKey(
                        name: "FK_Facturas_CLIENTES_id_cliente",
                        column: x => x.id_cliente,
                        principalSchema: "crm",
                        principalTable: "CLIENTES",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Facturas_RESERVAS_id_reserva",
                        column: x => x.id_reserva,
                        principalSchema: "ventas",
                        principalTable: "RESERVAS",
                        principalColumn: "id_reserva",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BOLETO",
                schema: "ventas",
                columns: table => new
                {
                    id_boleto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false, defaultValueSql: "decode('00000001','hex')"),
                    id_reserva = table.Column<int>(type: "integer", nullable: false),
                    id_vuelo = table.Column<int>(type: "integer", nullable: false),
                    id_asiento = table.Column<int>(type: "integer", nullable: true),
                    id_factura = table.Column<int>(type: "integer", nullable: false),
                    codigo_boleto = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    clase = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    precio_vuelo_base = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    precio_asiento_extra = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    impuestos_boleto = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    cargo_equipaje = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    precio_final = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    estado_boleto = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    fecha_emision = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    es_eliminado = table.Column<bool>(type: "boolean", nullable: false),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modificacion_ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOLETO", x => x.id_boleto);
                    table.ForeignKey(
                        name: "FK_BOLETO_ASIENTO_id_asiento",
                        column: x => x.id_asiento,
                        principalSchema: "vuelos",
                        principalTable: "ASIENTO",
                        principalColumn: "id_asiento",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BOLETO_Facturas_id_factura",
                        column: x => x.id_factura,
                        principalSchema: "ventas",
                        principalTable: "Facturas",
                        principalColumn: "id_factura",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BOLETO_RESERVAS_id_reserva",
                        column: x => x.id_reserva,
                        principalSchema: "ventas",
                        principalTable: "RESERVAS",
                        principalColumn: "id_reserva",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BOLETO_Vuelo_id_vuelo",
                        column: x => x.id_vuelo,
                        principalSchema: "vuelos",
                        principalTable: "Vuelo",
                        principalColumn: "id_vuelo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EQUIPAJE",
                schema: "ventas",
                columns: table => new
                {
                    id_equipaje = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false, defaultValueSql: "decode('00000001','hex')"),
                    id_boleto = table.Column<int>(type: "integer", nullable: false),
                    tipo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    peso_kg = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    descripcion_equipaje = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    precio_extra = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    dimensiones_cm = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    numero_etiqueta = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    estado_equipaje = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    es_eliminado = table.Column<bool>(type: "boolean", nullable: false),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    creado_por_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    fecha_registro_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modificado_por_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fecha_modificacion_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modificacion_ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EQUIPAJE", x => x.id_equipaje);
                    table.ForeignKey(
                        name: "FK_EQUIPAJE_BOLETO_id_boleto",
                        column: x => x.id_boleto,
                        principalSchema: "ventas",
                        principalTable: "BOLETO",
                        principalColumn: "id_boleto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AEROPUERTO_id_ciudad",
                schema: "aero",
                table: "AEROPUERTO",
                column: "id_ciudad");

            migrationBuilder.CreateIndex(
                name: "IX_AEROPUERTO_id_pais",
                schema: "aero",
                table: "AEROPUERTO",
                column: "id_pais");

            migrationBuilder.CreateIndex(
                name: "UQ_AEROPUERTO_CODIGO_IATA",
                schema: "aero",
                table: "AEROPUERTO",
                column: "codigo_iata",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_ASIENTO_VUELO_NUMERO",
                schema: "vuelos",
                table: "ASIENTO",
                columns: new[] { "id_vuelo", "numero_asiento" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AUDITORIA_FECHA",
                schema: "crm",
                table: "AUDITORIA_LOG",
                column: "fecha_evento_utc");

            migrationBuilder.CreateIndex(
                name: "IX_AUDITORIA_TABLA",
                schema: "crm",
                table: "AUDITORIA_LOG",
                column: "tabla_afectada");

            migrationBuilder.CreateIndex(
                name: "UQ_AUDITORIA_GUID",
                schema: "crm",
                table: "AUDITORIA_LOG",
                column: "auditoria_guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BOLETO_id_asiento",
                schema: "ventas",
                table: "BOLETO",
                column: "id_asiento");

            migrationBuilder.CreateIndex(
                name: "IX_BOLETO_id_factura",
                schema: "ventas",
                table: "BOLETO",
                column: "id_factura");

            migrationBuilder.CreateIndex(
                name: "IX_BOLETO_id_reserva",
                schema: "ventas",
                table: "BOLETO",
                column: "id_reserva");

            migrationBuilder.CreateIndex(
                name: "IX_BOLETO_id_vuelo",
                schema: "ventas",
                table: "BOLETO",
                column: "id_vuelo");

            migrationBuilder.CreateIndex(
                name: "UQ_BOLETO_CODIGO",
                schema: "ventas",
                table: "BOLETO",
                column: "codigo_boleto",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_CIUDAD_PAIS_NOMBRE",
                schema: "aero",
                table: "CIUDAD",
                columns: new[] { "id_pais", "nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CLIENTE_CORREO",
                schema: "crm",
                table: "CLIENTES",
                column: "correo");

            migrationBuilder.CreateIndex(
                name: "IX_CLIENTES_id_ciudad_residencia",
                schema: "crm",
                table: "CLIENTES",
                column: "id_ciudad_residencia");

            migrationBuilder.CreateIndex(
                name: "IX_CLIENTES_id_pais_nacionalidad",
                schema: "crm",
                table: "CLIENTES",
                column: "id_pais_nacionalidad");

            migrationBuilder.CreateIndex(
                name: "UQ_CLIENTE_GUID",
                schema: "crm",
                table: "CLIENTES",
                column: "cliente_guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_CLIENTE_IDENTIFICACION",
                schema: "crm",
                table: "CLIENTES",
                column: "numero_identificacion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EQUIPAJE_id_boleto",
                schema: "ventas",
                table: "EQUIPAJE",
                column: "id_boleto");

            migrationBuilder.CreateIndex(
                name: "UQ_EQUIPAJE_ETIQUETA",
                schema: "ventas",
                table: "EQUIPAJE",
                column: "numero_etiqueta",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Escala_id_aeropuerto",
                schema: "vuelos",
                table: "Escala",
                column: "id_aeropuerto");

            migrationBuilder.CreateIndex(
                name: "UQ_Escala_Vuelo_Orden",
                schema: "vuelos",
                table: "Escala",
                columns: new[] { "id_vuelo", "orden" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_id_cliente",
                schema: "ventas",
                table: "Facturas",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_id_reserva",
                schema: "ventas",
                table: "Facturas",
                column: "id_reserva");

            migrationBuilder.CreateIndex(
                name: "UQ_FACTURA_GUID",
                schema: "ventas",
                table: "Facturas",
                column: "guid_factura",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_FACTURA_NUMERO",
                schema: "ventas",
                table: "Facturas",
                column: "numero_factura",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pasajero_id_cliente",
                schema: "ventas",
                table: "Pasajero",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_RESERVAS_id_asiento",
                schema: "ventas",
                table: "RESERVAS",
                column: "id_asiento");

            migrationBuilder.CreateIndex(
                name: "IX_RESERVAS_id_cliente",
                schema: "ventas",
                table: "RESERVAS",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_RESERVAS_id_pasajero",
                schema: "ventas",
                table: "RESERVAS",
                column: "id_pasajero");

            migrationBuilder.CreateIndex(
                name: "IX_RESERVAS_id_vuelo",
                schema: "ventas",
                table: "RESERVAS",
                column: "id_vuelo");

            migrationBuilder.CreateIndex(
                name: "UQ_ROL_GUID",
                schema: "seg",
                table: "ROL",
                column: "rol_guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_ROL_NOMBRE",
                schema: "seg",
                table: "ROL",
                column: "nombre_rol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_APP_correo",
                schema: "seg",
                table: "USUARIO_APP",
                column: "correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_APP_id_cliente",
                schema: "seg",
                table: "USUARIO_APP",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_APP_username",
                schema: "seg",
                table: "USUARIO_APP",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_APP_usuario_guid",
                schema: "seg",
                table: "USUARIO_APP",
                column: "usuario_guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USUARIOS_ROLES_ROL",
                schema: "seg",
                table: "USUARIOS_ROLES",
                column: "id_rol");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIOS_ROLES_USUARIO",
                schema: "seg",
                table: "USUARIOS_ROLES",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "UQ_USUARIOS_ROLES_USR_ROL",
                schema: "seg",
                table: "USUARIOS_ROLES",
                columns: new[] { "id_usuario", "id_rol" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vuelo_id_aeropuerto_destino",
                schema: "vuelos",
                table: "Vuelo",
                column: "id_aeropuerto_destino");

            migrationBuilder.CreateIndex(
                name: "IX_Vuelo_id_aeropuerto_origen",
                schema: "vuelos",
                table: "Vuelo",
                column: "id_aeropuerto_origen");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AUDITORIA_LOG",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "EQUIPAJE",
                schema: "ventas");

            migrationBuilder.DropTable(
                name: "Escala",
                schema: "vuelos");

            migrationBuilder.DropTable(
                name: "USUARIOS_ROLES",
                schema: "seg");

            migrationBuilder.DropTable(
                name: "BOLETO",
                schema: "ventas");

            migrationBuilder.DropTable(
                name: "ROL",
                schema: "seg");

            migrationBuilder.DropTable(
                name: "USUARIO_APP",
                schema: "seg");

            migrationBuilder.DropTable(
                name: "Facturas",
                schema: "ventas");

            migrationBuilder.DropTable(
                name: "RESERVAS",
                schema: "ventas");

            migrationBuilder.DropTable(
                name: "ASIENTO",
                schema: "vuelos");

            migrationBuilder.DropTable(
                name: "Pasajero",
                schema: "ventas");

            migrationBuilder.DropTable(
                name: "Vuelo",
                schema: "vuelos");

            migrationBuilder.DropTable(
                name: "CLIENTES",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "AEROPUERTO",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "CIUDAD",
                schema: "aero");

            migrationBuilder.DropTable(
                name: "Pais",
                schema: "aero");
        }
    }
}
