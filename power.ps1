param($t,$u,$p)

#docker build 
Write-Host "Hello $t"

docker login --username=$u registry.cn-hangzhou.aliyuncs.com --password=$p

$ServiceDockerfilePath="./src/Services/Masa.PM.Service.Admin/Dockerfile"
$ServiceServerName="masa-pm-service-admin"
$WebDockerfilePath="./src/Web/Masa.PM.Web.Admin/Dockerfile"
$WebServerName="masa-pm-web-admin"

docker build -t registry.cn-hangzhou.aliyuncs.com/masastack/${ServiceServerName}:$t  -f $ServiceDockerfilePath .
docker push registry.cn-hangzhou.aliyuncs.com/masastack/${ServiceServerName}:$t 

docker build -t registry.cn-hangzhou.aliyuncs.com/masastack/${WebServerName}:$t  -f $WebDockerfilePath .
docker push registry.cn-hangzhou.aliyuncs.com/masastack/${WebServerName}:$t