pipeline {
    agent any
    
    stages {
        stage ("test") {
            steps {
                script {
                    echo "testing .net app"
                }
            }
        }
    }
    stages {
        stage ("build") {
            steps {
                script {
                    echo "building .net app"
                }
            }
        }
    }
    stages {
        stage ("deploy") {
            steps {
                script {
                    echo "deploying the .net app"
                }
            }
        }
    }
}