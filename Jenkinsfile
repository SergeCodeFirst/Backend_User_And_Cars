#!/user/bin/env groovy
// library identifier: 'jenkins-shared-library_UserAndCars@main', retriever: modernSCM(
//     [$class: 'GitSCMSource',
//     remote: 'https://github.com/SergeCodeFirst/jenkins-shared-library_UserAndCars.git',
//     credentialsId: 'github-credentials'
//     ])

// trigger #3

@Library('jenkins-shared-library_UserAndCars')_

pipeline {
    agent any
    
    tools {
        dotnetsdk 'dotnet-7.0'
        maven "maven-3.9"
    }
    
    stages {
        stage ("Increment app version") {
            steps {
                script {
                    echo 'incrementing app version ...'
                    sh 'chmod +x update_version.sh'
                    sh 'chmod +rw backend.csproj'
                    sh './update_version.sh patch'
                    def matcher = readFile('backend.csproj') =~ '<Version>(.+)</Version>'
                    def version = matcher[0][1]
                    env.IMAGE_NAME = "$version-$BUILD_NUMBER"
                }
            }
        }
        
        stage ("build Application Artifact") {
            steps {
                script {
                    buildAppArtifact()
                }
            }
        }
        
        stage ("build and push docker image ") {
            steps {
                script {
                    buildImage "sergevismok/demo-app:dotnet-app-${IMAGE_NAME}"
                    dockerLogin()
                    dockerPush "sergevismok/demo-app:dotnet-app-${IMAGE_NAME}"
                }
            }
        }
        
        stage ("deploy") {
            steps {
                script {
                    def dockerCmd = 'docker run -p 5000:80 -d sergevismok/demo-app:dotnet-app-1.0.6-41'
                    // deployApp()
                    sshagent(['ec2-server-key']) {
                        // some block
                        sh "ssh -o StrictHostKeyChecking=no ec2-user@18.222.227.60 ${dockerCmd}"
                    }
                }
            }
        }

        stage ("commit version update") {
            steps {
                script {
                    // used PAT token ass a password in the Jenkins-push crdentials
                    withCredentials([usernamePassword(credentialsId: 'Jenkins-push', passwordVariable: 'PAT', usernameVariable: 'USER')]) {
                        sh 'git config --global user.email "jenkins@example.com"'
                        sh 'git config --global user.name "jenkins"'
                        
                        sh 'git status'
                        sh 'git branch'
                        sh 'git config --list'
                        
                        sh "git remote set-url origin https://${USER}:${PAT}@github.com/SergeCodeFirst/Backend_User_And_Cars.git"
                        sh 'git add .'
                        sh 'git commit -m "ci: version bump"'
                        sh 'git push origin HEAD:main'
                    }
                }
            }
        }
    }
}



// =================================
// With everything in jenkins
// =================================

// pipeline {
//     agent any
    
//     tools {
//         dotnetsdk 'dotnet-7.0'
//         // maven 'maven-3.9'
//         // nodejs 'node-14.0'
//         // python 'Python-3.8'
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



// =================================
// // With function Architecture
// =================================

// def gv
// pipeline {
//     agent any
    
//     tools {
//         dotnetsdk 'dotnet-7.0'
//         maven "maven-3.9"
//     }
    
//     stages {
//         stage ("init") {
//             steps {
//                 script {
//                     gv = load "groovyScript/script.groovy"
//                 }
//             }
//         }
//         stage ("build Application Artifact") {
//             steps {
//                 script {
//                     gv.buildAppArtifact()
//                 }
//             }
//         }
        
//         stage ("build docker image") {
//             steps {
//                 script {
//                     gv.buildDockerImage()
//                 }
//             }
//         }
        
//         stage ("deploy") {
//             steps {
//                 script {
//                     gv.deployApp()
//                 }
//             }
//         }
//     }
// }

// =================================
// With jenkins shared library
// =================================


