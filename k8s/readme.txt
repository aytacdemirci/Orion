1-) Ana dizinde 
    - docker build -f src\Web\Orion.API\Dockerfile --force-rm -t aytac3737/orionapp:v1 . 
    - docker push aytac3737/orionapp:v1
    - http://localhost:30035/swagger/index.html

Kubectl apply -f k8s
postmanden http://localhost:30035/api/stories adresine get isteği atarsak çalışacaktır.http://localhost:30035/swagger/index.html


Not
dotnetapp-service-deployment.yaml içerisindeki yorum satırı açılırsa http://localhost:80/api/stories şeklinde kullanılmalı