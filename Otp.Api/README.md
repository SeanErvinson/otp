## Install mssql in a container

```shell
sudo nerdctl run \
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

## Install azurite in a container

```shell
sudo nerdctl run \
--name azurite-otp \
-d \
--restart always \
-p 10000:10000 \
-p 10001:10001 \
-p 10002:10002 \
-v ~/RiderProjects/Storage/otp-dev/azurite:/data \
mcr.microsoft.com/azure-storage/azurite
```
