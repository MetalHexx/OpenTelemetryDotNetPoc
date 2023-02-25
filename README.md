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

- Vendor agnostic instrumentation through use of an Otel collector gateway and otel exporters on api 
- Simulated api response time delays for variation
- Simulated errors with random exceptions thrown (500 status code) for variation
- Api filters to capture metrics with route, class, method, and status code labels to use for querying
- Metric processor to enrich metric labels
- Signal correlation with grafana -- see: Grafana Section

### Issues
- (FIXED) - Experiencing delayed metrics export with otel exporter compared to Prometheus exporter.   See Stack overflow: https://stackoverflow.com/questions/75552005/opentelemetry-net-application-metrics-collected-slowly-by-collector

### Future Goals:
- Continue improving signal correlation in Grafana
- Instrument logging stack.  Maybe give Loki a try.
- Instrument Zipkin to compare to Jaeger
- Add load testing tooling to simulate a constant load for better demonstration
- Add a collector agent

## Docker Instructions:
- From Visual Studio: Select and run project with Docker Compose Startup project in Visual Studio
- From CLI:  docker compose up

## UI Endpoints to check out:
- Swagger: http://localhost:5000/swagger/index.html
- Prometheus: http://localhost:9090/graph
- Collector Prometheus Export Endpont: http://localhost:8889
- Jaeger: http://localhost:16686/search
- Grafana: http://localhost:3000

## Grafana 
### Instructions:
- Navigate to http://localhost:3000 and login with admin/admin
- Add prometheus datasource with host: http://poc-prometheus:9090
- Add Jaeger datasource with host: http://poc-jaeger:16686
- Set the Jager datasource to scrape every 1s for best demo effect
- Go to /telemetry/grafana folder and copy contents of poc-dashboard.json 
- Create a new dashboard with "Import" and paste json
### Features
- Ability to filter metrics by metric tag variables. 
- Ability to correlate Metrics and Traces by time range, route, or status code.
  - Traces will not display if multiple values are selected on a given variable.  
  - Make sure to only select a single value per variable for now if you want to see traces.

#### Filter Metrics With Variables

<img src="https://github.com/MetalHexx/OpenTelemetryDotNetPoc/blob/main/assets/grafana.png?raw=true" style=" width:40% ; height:40% " >

#### Trace Correlation (limited to 1 value per variable right now)

<img src="https://github.com/MetalHexx/OpenTelemetryDotNetPoc/blob/main/assets/grafana-2.png?raw=true" style=" width:40% ; height:40% " >

#### Click into traces to get to rich information about the request.

<img src="https://github.com/MetalHexx/OpenTelemetryDotNetPoc/blob/main/assets/grafana-trace-details.png?raw=true" style=" width:50% ; height:50% " >
