BP Calculator – Project README
Overview

This project extends the BPcalculator application by fixing missing logic, improving the UI, adding new analytical features, and implementing a complete CI/CD pipeline with automated testing, telemetry, security scanning, and blue–green deployment.
The work follows a full modern software engineering workflow, including unit testing, BDD, E2E testing, performance testing, and continuous deployment to Azure.
Features Implemented
Core Enhancements
Fixed missing and incorrect blood pressure categorisation logic
Added UI cosmetic improvements
Introduced Cardiovascular Risk Assessment feature
Implemented Dynamic Chart Rendering for visualising calculated results
Added Clear Button to reset form inputs
Quality & Testing
Unit tests implemented using xUnit
Behaviour-driven tests added using BDD scenarios
NUnit E2E tests to validate UI-level interaction
k6 performance testing for load and stress validation
Enforced ≥80% code coverage gate
DevOps, CI/CD & Deployment
Full CI pipeline via .github/workflows/dotnet.yml
Deployment to Azure Web App (Production)
Application Insights telemetry integration
SonarCloud static analysis integrated
Code formatting and linting via .editorconfig and StyleCop analyzers
Security scanning + automated dependency updates via Dependabot
Test & coverage artifacts uploaded:
TestResults.trx
Development Process – Step by Step
1. Fixing Logic & UI Improvements
Corrected missing blood pressure calculation logic
Applied UI enhancements for clarity and usability
2. CI Pipeline Setup
Created .github/workflows/dotnet.yml
Configured build, test, and artifact steps
3. New Features Added
Cardiovascular Risk Assessment
Dynamic Chart Rendering
Clear page/reset button
4. Test Implementation
Added xUnit unit tests and integrated them into CI
Added BDD tests and included them in pipeline execution
Wrote NUnit-based E2E tests for application-level flows
5. Deployment to Azure
Published CI artifacts
Deployed to Azure Web App
Verified successful execution through the domain link
6. Observability & Code Quality
Integrated Application Insights telemetry
Added SonarCloud integration for code quality and static analysis
Enforced 80%+ test coverage threshold
Applied formatting & style rules (StyleCop, .editorconfig)
Added security scanning and enabled Dependabot
Blue–Green Deployment Implementation
Because Azure Student subscriptions do not allow deployment slot creation, a blue–green strategy was implemented using two separate Web Apps:
14. Staging Environment Setup
Created a second Web App to act as staging
Pipeline configured to deploy CI artifacts to this staging app
15. Deploy CI Artifact to Staging
Added a staging environment in GitHub
Used azure/webapps-deploy@v2 with slot-style configuration
Staging receives all builds before Production
16. Smoke Tests on Staging
After staging deployment:
Performed HTTP GET checks on / and /Index
Verified 200 responses and expected content
17. Approval Gate + Production Deployment (Blue–Green Swap)
Configured manual approval in GitHub’s production environment
Second job waits for approval
Upon approval:
Deployed same artifact to Production
Simulates a slot swap (green → blue promotion)
18. Branch Protection
Enabled protection rules on main/master
Ensures all checks pass before merging
19. Environment Protection
Added reviewers for Production deployments
Prevents accidental or unauthorised release
Pipeline Summary
Stage	Tools / Methods
Build & Restore	.NET CLI
Unit Tests	xUnit
BDD Tests	SpecFlow / BDD
E2E Tests	NUnit
Performance Tests	k6
Code Coverage	Coverlet (≥80% gate)
Static Analysis	SonarCloud
Logging & Telemetry	Application Insights
Security	Dependabot + Vulnerability scanning
Deployment	Azure Web App + Blue–Green model
Technologies Used
.NET 8 / Razor Pages
xUnit, NUnit, SpecFlow
k6 (performance testing)
GitHub Actions (CI/CD)
Azure Web Apps
Application Insights
SonarCloud
Dependabot
StyleCop / .editorconfig

Deployment
Deployment is fully automated through GitHub Actions.
Every push triggers:
Build → Test → Coverage
SonarCloud analysis
Deploy to staging
Smoke tests
Manual approval
Deploy to production