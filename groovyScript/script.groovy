def testApp() {
    echo "Testing the .net app"
}

def buildAppArtifact() {
    echo 'building a dotnet artifact...'
    sh 'dotnet restore'
    sh 'dotnet publish -c Release'
}

def buildDockerImage() {
    echo 'build docker Image using artifact...'
    withCredentials([usernamePassword(credentialsId: 'docker-hub-repo', passwordVariable: 'PASS', usernameVariable: 'USER')]) {
        sh 'docker build -t sergevismok/demo-app:dotnet-app-2.0 .'
        sh 'echo $PASS | docker login -u $USER --password-stdin'
        sh 'docker push sergevismok/demo-app:dotnet-app-2.0'
    }
}

def deployApp() {
    echo "deploying the .net app..."
}

return this