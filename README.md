# API de identidade com registro e login de usuários com Identity. 
- Desenvolvida com AspNet Core 
- Banco de dados MySql rodando no container Docker
- Autenticação com Jwt Bearer
- Tratamento das mensagens de erro do Identity para pt-br
- Documentação da API com Swagger

Antes de subir a aplicação necessário subir o container docker com o banco MySql com o comando:
docker run --name mysql -d -p 3306:3306 -e MYSQL_ROOT_PASSWORD=senhaRoot@ -e MYSQL_DATABASE=identidade mysql:latest
