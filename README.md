# OpenTelemetryDotNetPoc
A small POC project to showcase instrumentation of a Net 6+ applications with OpenTelemetry.  This is an educational experiment and is not meant to represent a production-ready environment.  Some of this information is a little challenging to research and piece together. I thought this POC might be helpful as documentation for me and for those trying to get a jump start.

There are many ways to instrument this tooling. I will update this README.md to describe the overall approach as it evolves.  

## Approach
### Ingredients:
- Net 6 Web Api w/Controllers
- Docker Compose
- System.Diagnostics (uses open telemetry standards)
- OpenTelemetry .net packages
- OpenTelemetry Collector
- Prometheus
- Jaeger
- Grafana
- Influx Db (for Grafana)

### Current Setup:
<img src="https://github.com/MetalHexx/OpenTelemetryDotNetPoc/blob/main/assets/current-infrastructure.png?raw=true" style=" width:40% ; height:40% " >

- Simulated api response time delays for variation
- Simulated errors with random exceptions thrown (500 status code) for variation
- Api filters to capture metrics with route, class, method, and status code labels to use for querying
- Metric processor to enrich metric labels
- Signal correlation with grafana -- see: Grafana Section

### Future Goals:
- Continue making infrastructure vendor agnostic with collector
- Continue improving signal correlation in Grafana
- Instrument logging stack.  Maybe give Loki a try.
- Instrument Zipkin to compare to Jaeger
- Add load testing tooling to simulate a constant load for better demonstration

## Docker Instructions:
- Select and run project with Docker Compose Startup project in Visual Studio.  
- Currently experiencing collector scraping issues when using command line docker compose up.  Will fix soon.

## UI Endpoints to check out:
- Swagger: https://localhost:5001/swagger/index.html
- Prometheus: http://localhost:9090/graph
- Jaeger: http://localhost:16686/search
- Grafana: http://localhost:3000

## Grafana 
### Instructions:
- Navigate to http://localhost:3000 and login with admin/admin
- Add prometheus datasource with host: http://poc-prometheus:9090
- Add Jaeger datasource with host: http://poc-jaeger:16686
- Go to /telemetry/grafana folder and copy contents of poc-dashboard.json 
- Create a new dashboard with "Import" and paste json
### Features
- Ability to filter metrics by metric tag variables. 
- Ability to correlate Metrics and Traces by time range, route, or status code.
  - Traces will not display if multiple values are selected on a given variable.  
  - Make sure to only select single values for now.

<img src="https://github.com/MetalHexx/OpenTelemetryDotNetPoc/blob/main/assets/grafana.png?raw=true" style=" width:40% ; height:40% " >

Click into traces to get to rich information about the request.

<img src="https://github.com/MetalHexx/OpenTelemetryDotNetPoc/blob/main/assets/grafana-trace-details.png?raw=true" style=" width:50% ; height:50% " >
