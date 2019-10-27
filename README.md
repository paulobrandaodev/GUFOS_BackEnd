# GUFOS - Agenda de Eventos - BackEnd C# - .Net Core 3.0

## Requisitos
> - Visual Studio Code <br>
> - Banco de Dados funcionando - DDLs, DMLs e DQLs <br>
> - .NET Core SDK 3.0

## Criação do Projeto
> Criamos nosso projeto de API com: 
```bash
dotnet new webapi
```
<br>

## Entity Framework - Database First

> Instalar o gerenciador de pacotes EF na máquina:
```bash
dotnet tool install --global dotnet-ef
```

<br>

> Baixar Pacote SQL Server:
```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

<br>

> Baixar pacote de escrita de códigos do EF:
```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
```

<br>

> Dar um restore na aplicação para ler e aplicar os pacotes instalados:
```bash
dotnet restore
```

<br>

> Testar se o EF está ok
```bash
dotnet ef
```

<br>

> Criar os Models à partir da sua base de Dados
    :point_right: -o = criar o diretorio caso não exista
    :point_right: -d = Incluir as DataNotations do banco
```bash
dotnet ef dbcontext scaffold "Server=DESKTOP-XVGT587\SQLEXPRESS;Database=Gufos;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -d
```
<br>

## Controllers
> Apagamos o controller que já vem com a base...

### CategoriaController

> Criamos nosso primeiro Controller: CategoriaController <br>
> Herdamos nosso novo controller de ControllerBase <br>
> Definimos a "rota" da API logo em cima do nome da classe, utilizando:
```c#
[Route("api/[controller]")]
```
> Logo abaixo dizemos que é um controller de API, utilizando:
```c#
[ApiController]
```
<br>

> Damos **CTRL + .** para incluir:

```c#
using Microsoft.AspNetCore.Mvc;
```
<br>

> Instanciamos nosso contexto da nossa Base de Dados:
```c#
GufosContext _contexto = new GufosContext();
```

> Damos **CTRL + .** para incluir nossos models:
```c#
using GUFOS_BackEnd.Models;
```

> Criamos nosso método **GET**:
```c#
        // GET: api/Categoria/
        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> Get()
        {
            var categorias = await _context.Categoria.ToListAsync();

            if (categorias == null)
            {
                return NotFound();
            }

            return categorias;
        }
```

> Importamos com CTRL + . as dependências:
```c#
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
```
<br>

> Testamos o método GET de nosso controller no Postman:
```bash
dotnet run
https://localhost:5001/api/categoria
```

> Deve ser retornado:
```json
[
    {
        "categoriaId": 1,
        "titulo": "Desenvolvimento",
        "evento": []
    },
    {
        "categoriaId": 2,
        "titulo": "HTML + CSS",
        "evento": []
    },
    {
        "categoriaId": 3,
        "titulo": "Marketing",
        "evento": []
    }
]
```

<br>

> Criamos nossa sobrecarga de método **GET**, desta vez passando como parâmetro o ID:
```c#
        // GET: api/Categoria/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> Get(int id)
        {
            var categoria = await _context.Categoria.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }
```
> Testamos no Postman: [https://localhost:5001/api/categoria/1](https://localhost:5001/api/categoria/1)

<br>

> Criamos nosso método **POST** para inserir uma nova categoria:
```c#
        // POST: api/Categoria/
        [HttpPost]
        public async Task<ActionResult<Categoria>> Post(Categoria categoria)
        {
            try
            {
                await _context.AddAsync(categoria);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return categoria;
        }
```
> Testamos no Postman, passando em RAW , do tipo JSON:
```json
{
    "titulo": "Teste"
}
```

<br>

> Criamos nosso método **PUT** para atualizar os dados:
```c#
        // PUT: api/Categoria/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest();
            }

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var categoria_valido = await _context.Categoria.FindAsync(id);

                if (categoria_valido == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
```
> Testamos no Postman, no método PUT, pela URL [https://localhost:5001/api/categoria/4](https://localhost:5001/api/categoria/4) passando:
```json
{
    "categoriaId": 4,
    "titulo": "Design Gráfico"
}
```

<br>

> Por último, incluímos nosso método **DELETE** , para excluir uma determinada Categoria:
```c#
        // DELETE: api/Categoria/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Categoria>> Delete(int id)
        {
            var categoria = await _context.Categoria.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            _context.Categoria.Remove(categoria);
            await _context.SaveChangesAsync();

            return categoria;
        }
```
> Testamos pelo Postman, pelo mérodo DELETE, e com a URL: [https://localhost:5001/api/categoria/4](https://localhost:5001/api/categoria/4) <br>
> Deve-se retornar o objeto excluído:
```json
{
    "categoriaId": 4,
    "titulo": "Design Gráfico",
    "evento": []
}
```

<br>

### LocalizacaoController
> Copiar ControllerCategoria e alterar com **CTRL + F** <br>
> Testar os métodos REST

<br>

### EventoController
> Copiar ControllerCategoria e alterar com **CTRL + F** <br>
> Testar os métodos REST <br>
> Notamos que no método **GET** não retorna a árvore de objetos *Categoria* e *Localizacao* <br>
> Para incluirmos é necessário adicionar em nosso projeto o seguinte pacote:
```bash
dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson
```
> Depois em nossa Startup.cs, dentro de ConfigureServices, no lugar de *services.AddControllers()* :
```c#
services.AddControllersWithViews().AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
```
> Damos **CTRL + .** para incluir a dependência:
```c#
using Newtonsoft.Json;
```
> Após isso precisamos mudar nosso controller para receber os atributos, no método GET ficará assim:
```c#
var eventos = await _context.Evento.Include(c => c.Categoria).Include(l => l.Localizacao).ToListAsync();
```
> No método GET com parâmetro ficará assim:
```c#
var evento = await _context.Evento.Include(c => c.Categoria).Include(l => l.Localizacao).FirstOrDefaultAsync(e => e.EventoId == id);
```

<br>

> Adicionar os Controllers restantes

<br><br>

## SWAGGER - Documentação da API

>  Instalar o Swagger:
```bash
dotnet add Gufos_BackEnd.csproj package Swashbuckle.AspNetCore -v 5.0.0-rc4
```
<br>

> Registramos o gerador do Swagger dentro de ConfigureServices, definindo 1 ou mais documentos do Swagger:
```c#
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                // Mostrar o caminho dos comentários dos métodos Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
```

<br>

> Colocar na Startup com **CTRL + .**
```c#
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
```

<br>

> Colocar dentro do csproj para gerar a documentação com base nos comentários dos métodos:
```c#
  <PropertyGroup>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
    <NoWarn>$(NoWarn);1591</NoWarn>
  </PropertyGroup>
```

<br>

> Em Startup.cs , no método Configure, habilite o middleware para atender ao documento JSON gerado e à interface do usuário do Swagger:
```c#
            // Habilitamos efetivamente o Swagger em nossa aplicação.
            app.UseSwagger();
            // Especificamos o endpoint da documentação
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });
```

<br>

> Rodar a aplicação e testar em: [https://localhost:5001/swagger/](https://localhost:5001/swagger/)

<br>

## JWT - Autenticação com Json Web Token

> Instalar pacote JWT
```bash
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 3.0.0
```
<br>

> Adicionar a configuração do nosso Serviço de autenticação:
```c#
            // JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)  
            .AddJwtBearer(options =>  
            {  
                options.TokenValidationParameters = new TokenValidationParameters  
                {  
                    ValidateIssuer = true,  
                    ValidateAudience = true,  
                    ValidateLifetime = true,  
                    ValidateIssuerSigningKey = true,  
                    ValidIssuer = Configuration["Jwt:Issuer"],  
                    ValidAudience = Configuration["Jwt:Issuer"],  
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))  
                };  
            });
```

> Importar com **CTRL + .** as dependências:
```c#
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
```

<br>

> Adicionamos em nosso *appsettings.json* :
```json
{
  "Jwt": {  
    "Key": "GufosSecretKey",  
    "Issuer": "gufos.com"  
  },
}  
```

<br>

> Em Startup.cs , no método Configure , usamos efetivamente a autenticação:
```c#
app.UseAuthentication();
``` 

<br><br>

> Criamos o Controller *LoginController* e herdamos da *ControllerBase* <br>
> Colocamos a rota da API e dizemos que é um controller de API :
```c#
    [Route("api/[controller]")]  
    [ApiController] 
    public class LoginController : ControllerBase
    {
        
    }
```
<br>

> Criamos nossos métodos:
```c#
        // Chamamos nosso contexto do banco
        GufosContext _context = new GufosContext();

        // Definimos uma variável para percorrer nossos métodos com as configurações obtidas no appsettings.json
        private IConfiguration _config;  

        // Definimos um método construtor para poder passar essas configs
        public LoginController(IConfiguration config)  
        {  
            _config = config;  
        }

        // Chamamos nosso método para validar nosso usuário da aplicação
        private Usuario AuthenticateUser(Usuario login)  
        {  
            var usuario =  _context.Usuario.FirstOrDefault(u => u.Email == login.Email && u.Senha == login.Senha);
  
            if (usuario != null)  
            {  
                usuario = login;  
            }  

            return usuario;  
        }  

        // Criamos nosso método que vai gerar nosso Token
        private string GenerateJSONWebToken(Usuario userInfo)  
        {  
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));  
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Definimos nossas Claims (dados da sessão) para poderem ser capturadas
            // a qualquer momento enquanto o Token for ativo
            var claims = new[] {  
                new Claim(JwtRegisteredClaimNames.NameId, userInfo.Nome),  
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),  
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())  
            }; 

            // Configuramos nosso Token e seu tempo de vida
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],  
              _config["Jwt:Issuer"],  
              claims,  
              expires: DateTime.Now.AddMinutes(120),  
              signingCredentials: credentials);  
  
            return new JwtSecurityTokenHandler().WriteToken(token);  
        }  
  

        
        // Usamos essa anotação para ignorar a autenticação neste método, já que é ele quem fará isso  
        [AllowAnonymous]  
        [HttpPost]  
        public IActionResult Login([FromBody]Usuario login)  
        {  
            IActionResult response = Unauthorized();  
            var user = AuthenticateUser(login);  
  
            if (user != null)  
            {  
                var tokenString = GenerateJSONWebToken(user);  
                response = Ok(new { token = tokenString });  
            }  
  
            return response;  
        }
```

> Importamos as dependências:
```c#
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using GUFOS_BackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
```
<br>

> Testamos se está sendo gerado nosso Token pelo Postman, no método POST <br>
> Pela URL : [https://localhost:5001/api/login](https://localhost:5001/api/login) <br>
> E com os seguintes parâmetros pela RAW : 
```json
{
    "nome": "Administrador",
    "email": "adm@adm.com",
    "senha": "123",
}
```

> O retorno deve ser algo do tipo:
```json
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJQYXVsbyIsImVtYWlsIjoiYWRtQGFkbS5jb20iLCJqdGkiOiIwYjNmMGM3ZC1mMDhjLTQ4NDQtOTI1Mi04ZDI1ZTZmY2MxYmYiLCJleHAiOjE1NzExNTcyMjUsImlzcyI6Imd1Zm9zLmNvbSIsImF1ZCI6Imd1Zm9zLmNvbSJ9.bk_cvQJgVpq7TXa8Nhh1XzWAEUnTXHc2lP5vvqIVhJs"
}
```

> Após confirmar, vamos até [https://jwt.io/](https://jwt.io/) <br>
> Colamos nosso Token lá e em Payload devemos ter os seguintes dados:
```json
{
  "nameid": "Administrador",
  "email": "adm@adm.com",
  "jti": "d1e13b73-5f8f-423c-97e2-835f55bbfb0e",
  "exp": 1571157573,
  "iss": "gufos.com",
  "aud": "gufos.com"
}
```

<br><br>

> Pronto! Agora é só utilizar a anotação *[Authorize]* em baixo da anotação REST de cada método que desejar colocar autenticação! <br>
> No Postman devemos gerar um token pela rota de login e nos demais endpoints devemos adicionar o token gerado na aba *Authorization*  escolhendo a opção ***Baerer Token***


<br><br>

# Reestruturando projeto para os padrões de mercado
## Incluindo Domains, Repositories, Interfaces

> Apagamos a pasta Models e fazemos o scaffold novamente, com a nomenclatura Domains no lugar de Models

> Antes disso recortamos nossos Controllers para outro diretório para não dar erro de Build

```bash
dotnet ef dbcontext scaffold "Server=DESKTOP-XVGT587\SQLEXPRESS;Database=Gufos;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Domains -d
```

> Depois de gerados os domínios(models) recolocamos os controllers dentro do projeto

> Substituímos todos os "Usings" de nossos Controllers que estão como "Models" para "Domains"

<br>

### Interfaces
> Criamos nosso diretório "Interfaces"
> Dentro deste diretório criamos nossa primeira interface: "ICategoria.cs"
<br>

> Dentro de ICategoria criamos nossos "Contratos"(métodos que serão obrigatórios):
```c#
        Task<List<Categoria>> Listar();

        Task<Categoria> BuscarPorID(int id);

        Task<Categoria> Salvar(Categoria categoria);

        Task<Categoria> Alterar(Categoria categoria);

        Task<Categoria> Excluir(Categoria categoria);
```

<br>

> Criamos nosso diretório "Repositories"
> Dentro deste diretório criamos nosso primeiro repositório: "CategoriaRepository.cs"
> Herdamos esta classe de "Icategoria":

```c#
using GUFOS_BackEnd.Interfaces;

namespace GUFOS_BackEnd.Repositories
{
    public class CategoriaRepository : ICategoria
    {
        
    }
}
```

<br>

> Damos **CTRL + .** para implementar a interface em nosso repositório, e colocando "async" nos métodos, ficando assim:
```c#
        public async Task<IActionResult> Alterar(long id, Categoria categoria)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ActionResult<Categoria>> BuscarPorID()
        {
            throw new System.NotImplementedException();
        }

        public async Task<ActionResult<Categoria>> Excluir(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ActionResult<List<Categoria>>> Listar()
        {
            throw new System.NotImplementedException();
        }

        public async Task<ActionResult<Categoria>> Salvar(Categoria categoria)
        {
            throw new System.NotImplementedException();
        }
```
<br>

> Dentro do método Listar() colocamos nosso contexto utilizando using (que será responsável por abrir e fechar a conexão com o banco):
```c#
    using(GufosContext _context = new GufosContext()){
        
    }
```

> Dentro do Using, já retornamos nossa "query" :
```c#
    using(GufosContext _context = new GufosContext()){
        return await _context.Categoria.ToListAsync();
    }
```

> Tiramos a instância do nosso Context do Controller e substituímos pela nossa "CategoriaRepository":
```c#
//GufosContext _context = new GufosContext();
CategoriaRepository repositorio = new CategoriaRepository();
```
> Importamos nosso diretório de repositórios:
```c#
using GUFOS_BackEnd.Repositories;
```
<br><br>

> Deixamos nossas interfaces e respositorios sem o AcionResult:
### CategoriaRepository:
```c#
        public async Task<Categoria> Alterar(Categoria categoria)
        {
            using(GufosContext _context = new GufosContext()){
                _context.Entry(categoria).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            return categoria;
        }

        public async Task<Categoria> BuscarPorID(int id)
        {
            using(GufosContext _context = new GufosContext()){
                return await _context.Categoria.FindAsync(id);
            }
        }

        public async Task<Categoria> Excluir(Categoria categoria)
        {
            using(GufosContext _context = new GufosContext()){
                _context.Categoria.Remove(categoria);
                await _context.SaveChangesAsync();
                return categoria;                
            }
        }

        public async Task<List<Categoria>> Listar()
        {
            using(GufosContext _context = new GufosContext()){
                return await _context.Categoria.ToListAsync();
            }
        }

        public async Task<Categoria> Salvar(Categoria categoria)
        {
            using(GufosContext _context = new GufosContext()){
                await _context.AddAsync(categoria);
                await _context.SaveChangesAsync();
                return categoria;
            }
        }
    }
```

### ICategoria:
```c#
    Task<List<Categoria>> Listar();

    Task<Categoria> BuscarPorID(int id);

    Task<Categoria> Salvar(Categoria categoria);

    Task<Categoria> Alterar(Categoria categoria);

    Task<Categoria> Excluir(Categoria categoria);
```
<br><br>

## ViewModel
> Criamos um diretorio chamado ***ViewModel*** 
> Dentro dele criamos nossa classe ***LoginViewModel***
> Dentro da classe colocamos somente os atributos que serão necessários para fazer o login e suas devidas DataAnotations:
```c#
        // Data Annotations
        [Required]
        public string Email { get; set; }
        // definimos o tamanho do campo
        [StringLength(255, MinimumLength = 5)]
        public string Senha { get; set; }
```

<br>

> Dentro de Login Controller deixamos de receber as informações do objeto Usuario(que precisa passar os dados que estão como obrigatórios) e passamos a receber de nosso LoginViewModel:

```c#

        // Chamamos nosso método para validar nosso usuário da aplicação
        private Usuario AuthenticateUser(LoginViewModel login)  
        {  
            var usuario =  _context.Usuario.FirstOrDefault(u => u.Email == login.Email && u.Senha == login.Senha);
            return usuario;  
        }  



        // Usamos essa anotação para ignorar a autenticação neste método, já que é ele quem fará isso  
        [AllowAnonymous]  
        [HttpPost]  
        public IActionResult Login([FromBody]LoginViewModel login)  
        {  
            IActionResult response = Unauthorized();  
            var user = AuthenticateUser(login);  
  
            if (user != null)  
            {  
                var tokenString = GenerateJSONWebToken(user);  
                response = Ok(new { token = tokenString });  
            }  
  
            return response;  
        }


```





