// Top-level build file where you can add configuration options common to all sub-projects/modules.
buildscript {
  repositories {
    google()
    mavenCentral()
    jcenter()
  }
  dependencies {
    classpath("com.android.tools.build:gradle:4.1.2")
    classpath("com.vanniktech:gradle-maven-publish-plugin:0.13.0")
  }
}

allprojects {
  repositories {
    google()
    mavenCentral()
    jcenter()
  }
}

tasks.register<Delete>("clean") {
  delete(rootProject.buildDir)
}