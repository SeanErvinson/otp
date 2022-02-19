## Install mssql in a container
```shell
nerdctl run \
--name sqldb-otp \
-d \
--restart always \
-e 'ACCEPT_EULA=Y' \
-e 'SA_PASSWORD=P@ssword1' \
-v ~/RiderProjects/Databases/otp-dev/data:/var/opt/mssql/data \
-v ~/RiderProjects/Databases/otp-dev/log:/var/opt/mssql/log \
-v ~/RiderProjects/Databases/otp-dev/secrets:/var/opt/mssql/secrets \
-p 1433:1433 \
-u root \
mcr.microsoft.com/mssql/server:2019-latest
```