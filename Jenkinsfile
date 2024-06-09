// // With everything in jenkins
// pipeline {
//     agent any
    
//     tools {
//         dotnetsdk 'dotnet-7.0'
//     }
    
//     stages {
//         stage ("build dotnet artifcat") {
//             steps {
//                 script {
//                     echo 'building a dotnet artifact...'
//                     sh 'dotnet restore'
//                     sh 'dotnet publish -c Release'
//                 }
//             }
//         }
//         stage ("build docker Image") {
//             steps {
//                 script {
//                     echo 'build docker Image using artifact...'
//                     withCredentials([usernamePassword(credentialsId: 'docker-hub-repo', passwordVariable: 'PASS', usernameVariable: 'USER')]) {
//                         sh 'docker build -t sergevismok/demo-app:dotnet-app-2.0 .'
//                         sh 'echo $PASS | docker login -u $USER --password-stdin'
//                         sh 'docker push sergevismok/demo-app:dotnet-app-2.0'
//                     }
//                 }
//             }
//         }
        
//         stage ("deploy") {
//             steps {
//                 script {
//                     echo 'deploying the application...'
//                 }
//             }
//         }
//     }
// }




// With function Architecture
def gv
pipeline {
    agent any
    
    tools {
        dotnetsdk 'dotnet-7.0'
    }
    
    stages {
        stage ("init") {
            steps {
                script {
                    gv = load "groovyScript/script.groovy"
                }
            }
        }
        stage ("build Application Artifact") {
            steps {
                script {
                    gv.buildAppArtifact()
                }
            }
        }
        
        stage ("build docker image") {
            steps {
                script {
                    gv.buildDockerImage()
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