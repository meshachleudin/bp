Feature: Blood pressure classification
  As a user of the BP calculator
  I want to know my blood pressure category and risks
  So that I can understand my health status

  Scenario Outline: BP classification and risk
    Given my systolic pressure is <Systolic>
    And my diastolic pressure is <Diastolic>
    When I evaluate my blood pressure
    Then the category should be "<Category>"
    And the heart risk level should be "<HeartRisk>"
    And the cardiovascular risk should be "<CardioRisk>"

    Examples:
      | Systolic | Diastolic | Category | HeartRisk                                                | CardioRisk                                 |
      | 90       | 60        | Low      | Low risk - Maintain hydration and regular meals.        | Low long-term cardiovascular risk.         |
      | 120      | 80        | Ideal    | Healthy - Keep up the good work!                        | Moderate long-term cardiovascular risk.    |
      | 135      | 85        | PreHigh  | Moderate risk - Consider lifestyle adjustments.         | High long-term cardiovascular risk.        |
      | 160      | 95        | High     | High risk - Consult your doctor.                        | High long-term cardiovascular risk.        |
