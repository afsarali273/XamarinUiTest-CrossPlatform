#!/bin/bash
# This is a Xamarin Testcloud test upload shell script

### This will have to be updated when Xamarin.UITest is updated via NuGet.

### You will have to update these variables for your environment
export CURRENTDATE=`date +"%Y-%m-%d %I.%M.%S %p"`
export PRJLOC="C:\Users\bbdnet1454\Desktop\Projects\XTC\Mobile"
export IOS_DEVICE_ID="hlogzaogza/ios"
export TEST_ASSEMBLIES="${PRJLOC}\VodaiOSPRODLocal\VodaiOS\bin\Debug"
export APK="C:\Users\bbdnet1454\Desktop\Projects\XTC\MyVodacom_Hockey_9.9.ipa"
export SERIES="master"
export APPNAME="hlogzaogza/VSP_IOS"
export LOCALE=en_US
export TESTDATA1="${PRJLOC}\VodaiOSPRODLocal\VodaiOS\bin\Debug\Envtestdata"
export TESTDATA2="${PRJLOC}\VodaiOSPRODLocal\VodaiOS\bin\Debug\EnvtestOR"
export XMLRESULTLOC="${APPNAME}\XTCresults[${CURRENTDATE}].xml"
export Logloc="C:\Users\bbdnet1454\Desktop\Projects\X
TC\Log\[${CURRENTDATE}].log"


appcenter test run uitest --app "${APPNAME}" --devices "${IOS_DEVICE_ID}" --app-path "${APK}" --test-series "${SERIES}" --locale "${LOCALE}" --build-dir "${TEST_ASSEMBLIES}" --include "${TESTDATA1}" --include "${TESTDATA2}" 



