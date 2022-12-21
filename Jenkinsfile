properties([pipelineTriggers([githubPush()])])

pipeline {
    agent any

    stages {
        stage('hello') {
            steps {
                // Get some code from a GitHub repository
                echo 'hello'
            }
        }
		stage('workplacePath'){
			steps{
				echo "${env.WORKSPACE}"
			}
		}
        stage('deleteWorkspace') {
            steps {
                deleteDir()
            }
        }

        stage('clone') {
            steps {
                // Get some code from a GitHub repository
                git branch: 'master',
                url: 'https://github.com/ProductivityTools-PhotoGallery/ProductivityTools.PhotoGallery.Api.git'
            }
        }
        stage('build') {
            steps {
				echo 'starting bddduild'
                bat('dotnet publish ProductivityTools.PhotoGallery.Api.sln -c Release')
            }
        }
       

        stage('stopSiteOnIis') {
            steps {
                bat('%windir%\\system32\\inetsrv\\appcmd stop site /site.name:PhotoGallery')
            }
        }

        stage('deleteIisDir') {
            steps {
                retry(5) {
                    bat('if exist "C:\\Bin\\PhotoGallery" RMDIR /Q/S "C:\\Bin\\PhotoGallery"')
                }

            }
        }
        stage('copyIisFiles') {
            steps {
                bat('xcopy "PhotoGallery.Api\\ProductivityTools.PhotoGallery.Api\\bin\\Release\\net6.0\\publish\\" "C:\\Bin\\PhotoGallery\\" /O /X /E /H /K')				              
            }
        }

        stage('startMeetingsOnIis') {
            steps {
                bat('%windir%\\system32\\inetsrv\\appcmd start site /site.name:PhotoGallery')
            }
        }
        stage('byebye') {
            steps {
                // Get some code from a GitHub repository
                echo 'byebye'
            }
        }
    }
	post {
		always {
            emailext body: "${currentBuild.currentResult}: Job ${env.JOB_NAME} build ${env.BUILD_NUMBER}\n More info at: ${env.BUILD_URL}",
                recipientProviders: [[$class: 'DevelopersRecipientProvider'], [$class: 'RequesterRecipientProvider']],
                subject: "Jenkins Build ${currentBuild.currentResult}: Job ${env.JOB_NAME}"
		}
	}
}
