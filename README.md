# WEB API - ASP.NET e MYSQL

## DESCRIÇÃO

Utilizando [ASP.NET](http://ASP.NET) Core, MySQL, Dapper e Swagger.

Uma API é uma interface de programação utilizada na montagem de aplicações. Ela faz o papel de recuperar somente os dados requisitados de um banco de dados.

## CRIAÇÃO DO PROJETO

![Untitled](https://github.com/Guilherme-Maciel/readme_images/blob/master/webApiMysql/Untitled.png)

## ESTRUTURA DO PROJETO

![Untitled](https://github.com/Guilherme-Maciel/readme_images/blob/master/webApiMysql/Untitled1.png)

## CRIAÇÃO DO BANCO

```sql
/*
SQLyog Community v13.1.5  (64 bit)
MySQL - 5.7.34-37-log : Database - dtm_teste_guilherme
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
USE `dtm_teste_guilherme`;

/*Table structure for table `contato` */

DROP TABLE IF EXISTS `contato`;

CREATE TABLE `contato` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(100) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `number` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

/*Data for the table `contato` */

insert  into `contato`(`id`,`name`,`email`,`number`) values 
(3,'Miguel Alterado','guilherme5932.ms@gmail.com','222'),
(4,'Teste1','value2','value3'),
(5,'Guilherme','a@a.com','2213423'),
(6,'João','joao@gmail.com','214234524');

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
```

## RAIZ

`Startup.cs`

```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPISelect.Repository;

namespace WebAPISelect
{
    public class Startup
    {
        //Contructor - Default
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IConfiguration GetConfiguration()
        {
            return Configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
 
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPISelect", Version = "v1" });
            });

            //Dependência - Configuração para o funcionamento do Swagger
            services.AddScoped<IContatoRepository>(factory =>
            {
                //Utiliza o construct da classe ContatoRepository para atribuir à variável _connectionString 
                //as informações do banco pelo arquivo appsettings.json.
                return new ContatoRepository(Configuration.GetConnectionString("MySqlDbConnection"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPISelect v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
```

`appsettings.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "ConnectionStrings": {
    "MySqlDbConnection": "server=mysqldev.dtmsp.com.br;database=dtm_teste_guilherme;user id=DTM_ASPNET;password=M@dtmusr"
  },
  "AllowedHosts": "*"
}
```

## MODEL

`Contato.cs`

```csharp
//Modelo padrão de envio de dados da entidade CONTATOS
namespace WebAPISelect.Model
{
    public class Contato
    {
        public int Id { get; set; }
        public string Name { get; set;}
        public string Email { get; set;}
        public string Number { get; set;}
    }
}
```

## REPOSITORY

`IContatoRepository.cs`

Interface do repositório ContatoRepository.cs.

Uma interface é uma moldura, com métodos não implementados, que só serão nas classes que a utilizarem.

Parecida com as classes abstratas.

```csharp
using System.Collections.Generic;
using WebAPISelect.Model;

namespace WebAPISelect.Repository
{
    public interface IContatoRepository
    {
        //Assinatura do método GetAll
        IEnumerable<Contato> GetAll();
        //Assinatura método Get
        IEnumerable<Contato> Get(int id);
        //Assinatura método Delete
        IEnumerable<Contato> Delete(int id);
        //Assinatura método Insert
        IEnumerable<Contato> Insert(string name, string email, string number);
        //Assinatura método update
        IEnumerable<Contato> Update(int id, string name, string email, string number);
    }
}
```

`ContatoRepository.cs`

```csharp
using System.Collections.Generic;
using WebAPISelect.Model;
using MySql.Data.MySqlClient;
using Dapper;

namespace WebAPISelect.Repository
{
    //Classe principal utilizando a interface
    public class ContatoRepository : IContatoRepository
    {
        //Variável de conexão.
        private readonly string _connectionString;
           
        //Construtor da classe. (A conexão é definida no arquivo Startup.cs)
        public ContatoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Contato> Delete(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var sqlStatement = "DELETE FROM Contato WHERE id = @Id";

                return connection.Query<Contato>(sqlStatement, new { Id = id });
            }
   
        }

        public IEnumerable<Contato> Get(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var sqlStatement = "SELECT * FROM Contato WHERE id = @Id";

                return connection.Query<Contato>(sqlStatement, new { Id = id });
            }
        }

        //Criação da conexão com o banco e retorno de uma consulta
        public IEnumerable<Contato> GetAll()
        {
            using(MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                return connection.Query<Contato>("SELECT * FROM Contato");
            }
        }

        public IEnumerable<Contato> Insert(string name, string email, string number)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var sqlStatement = "INSERT INTO Contato (name, email, number) VALUES (@Name, @Email, @Number)";

                return connection.Query<Contato>(sqlStatement, new { Name = name, Email = email, Number = number});
            }

        }

        public IEnumerable<Contato> Update(int id, string name, string email, string number)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var sqlStatement = "UPDATE Contato SET name = @Name, email = @Email, number = @Number WHERE id = @Id";

                return connection.Query<Contato>(sqlStatement, new { Id = id, Name = name, Email = email, Number = number });
            }
        }
    }
}
```

## CONTROLLER

`ContatoController.cs`

```sql
using WebAPISelect.Model;
using WebAPISelect.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace WebAPISelect.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ContatoController : ControllerBase
    {
        //Dependêcia
        private readonly IContatoRepository _contatoRepository;

        public ContatoController(IContatoRepository contatoRepository)
        {
            _contatoRepository = contatoRepository;
        }

        //Método HTTP GET
        [HttpGet]
        [Produces(typeof(Contato))]
        //Executa a query SELECT
        public IActionResult Get()
        {
            var contatos = _contatoRepository.GetAll();

            if (contatos.Count() == 0)
                return NoContent();

            return Ok(contatos);

        }

        //Método DELETE
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var deleteContato = _contatoRepository.Delete(id);

            return Ok(deleteContato);
        }

        //Método POST
        [HttpPost]
        public IActionResult Insert(string name, string email, string number)
        {
            var insertContato = _contatoRepository.Insert(name, email, number);

            return Ok(insertContato);
        }

        //Método PUT
        [HttpPut("update/{id}")]
        public IActionResult Update(int id, string name, string email, string number)
        {
            var updateContato = _contatoRepository.Update(id, name, email, number);

            return Ok(updateContato);
        }

       [HttpGet("{id}")]
       public IActionResult Get(int id)
        {
            var selectContato = _contatoRepository.Get(id);

            if (selectContato.Count() == 0)
                return NoContent();

            return Ok(selectContato);

        }
    }
}
```

# REFERÊNCIAS

[Basic Insert Update and Delete with Dapper | ASP.NET Monsters (aspnetmonsters.com)](https://www.aspnetmonsters.com/2019/02/2019-02-04-basic-insert-update-delete-with-dapper/)

[Dapper With MySQL/PostgreSQL On .NET Core - .NET Core Tutorials (dotnetcoretutorials.com)](https://dotnetcoretutorials.com/2020/07/11/dapper-with-mysql-postgresql-on-net-core/)

[Criando uma WebAPI + .Net core + VS Code + MySql - Code FC](https://codefc.com.br/criando-uma-webapi-net-core-vs-code-mysql/)
