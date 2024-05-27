def gv
pipeline {
    agent any
    
    stages {
        stage ("init") {
            steps {
                script {
                    gv = load "groovyScript/script.groovy"
                }
            }
        }
        stage ("test") {
            steps {
                script {
                    gv.buildApp()
                }
            }
        }
        
        stage ("build") {
            steps {
                script {
                    gv.buildApp()
                }
            }
        }
        
        stage ("deploy") {
            steps {
                script {
                    gv.deployApp()
                }
            }
        }
    }
}