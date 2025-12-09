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

  # ------------------------------------------------------------------
  # CATEGORY BOUNDARIES
  # ------------------------------------------------------------------
  Scenario Outline: Category boundaries for systolic and diastolic
    Given I have entered a systolic value of <systolic> and a diastolic value of <diastolic>
    When I evaluate my blood pressure
    Then the category should be "<category>"

    Examples:
      | systolic | diastolic | category |
      |  90      |  60       | Low      |
      |  91      |  60       | Ideal    |
      |  90      |  61       | Ideal    |
      | 120      |  80       | Ideal    |
      | 121      |  80       | PreHigh  |
      | 120      |  81       | PreHigh  |
      | 139      |  89       | PreHigh  |
      | 140      |  89       | High     |
      | 139      |  90       | High     |

  Scenario Outline: Category at valid extreme ranges
    Given I have entered a systolic value of <systolic> and a diastolic value of <diastolic>
    When I evaluate my blood pressure
    Then the category should be "<category>"

    Examples:
      | systolic | diastolic | category |
      |  70      |  40       | Low      |
      | 190      | 100       | High     |

  # ------------------------------------------------------------------
  # INVALID VALUE VALIDATION (DataAnnotations)
  # ------------------------------------------------------------------
  Scenario Outline: Validation errors for invalid systolic values
    Given I have entered a systolic value of <systolic> and a diastolic value of 70
    When I validate the blood pressure reading
    Then the validation should fail with error "Invalid Systolic Value"

    Examples:
      | systolic |
      | 69       |
      | 0        |
      | 191      |
      | 1000     |

  Scenario Outline: Validation errors for invalid diastolic values
    Given I have entered a systolic value of 120 and a diastolic value of <diastolic>
    When I validate the blood pressure reading
    Then the validation should fail with error "Invalid Diastolic Value"

    Examples:
      | diastolic |
      | 39        |
      | 0         |
      | 101       |
      | 500       |

  # ------------------------------------------------------------------
  # CARDIOVASCULAR RISK MESSAGE BOUNDARIES
  # ------------------------------------------------------------------
  Scenario Outline: Long-term cardiovascular risk thresholds
    Given I have entered a systolic value of <Systolic> and a diastolic value of <Diastolic>
    When I evaluate my blood pressure
    Then the cardiovascular risk should be "<CardioRisk>"

    Examples:
      | Systolic | Diastolic | CardioRisk                                 |
      | 100      | 70        | Low long-term cardiovascular risk.         |
      | 110      | 80        | Moderate long-term cardiovascular risk.    |
      | 130      | 80        | High long-term cardiovascular risk.        |
      | 140      | 90        | High long-term cardiovascular risk.        |
