using System.Collections;
using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class ArcticMeltdownTest
{
    // Name of the menu scene and the target scene
    private const string MenuSceneName = "_MainMenu";
    private const string StorySceneName = "_Story";
    private const string ControlsSceneName = "_Controls";
    private const string Level1SceneName = "Level1";
    private const float TestTimeScale = 2f;

    // A Test behaves as an ordinary method
    // [Test]
    // public void CheckForStatsTracker()
    // {
        
    // }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestForStartButtonAndStatsTracker()
    {
        // Load the menu scene
        yield return SceneManager.LoadSceneAsync(MenuSceneName);

        // Check if the stats tracker exists in Main Menu
        GameObject statsTracker = GameObject.Find("StatsTracker");
        Assert.IsNotNull(statsTracker, "Stats tracker object not found in the Main Menu.");

        // Allow one frame for the scene to initialize
        yield return new WaitForSeconds(6);
        yield return null;

        // Find the start button
        GameObject startButton = GameObject.Find("StartButton"); // Replace with your actual button name
        Assert.IsNotNull(startButton, "Start button not found in the scene.");

        // Simulate a button press
        Button buttonComponent = startButton.GetComponent<Button>();
        Assert.IsNotNull(buttonComponent, "Button component not found on the start button.");
        buttonComponent.onClick.Invoke();

        // Allow some time for the scene transition
        yield return new WaitForSeconds(1); // Adjust this duration as necessary

        // Check if the Level1 scene is loaded
        Scene activeScene = SceneManager.GetActiveScene();
        Assert.AreEqual(Level1SceneName, activeScene.name, "The Level1 scene was not loaded after pressing the start button.");

        // Check if the stats tracker exists in Level1
        statsTracker = GameObject.Find("StatsTracker");
        Assert.IsNotNull(statsTracker, "Stats tracker object not found in Level1.");
    }

    [UnityTest]
    public IEnumerator TestForStoryButton()
    {
        // Load the menu scene
        yield return SceneManager.LoadSceneAsync(MenuSceneName);

        // Allow one frame for the scene to initialize
        yield return new WaitForSeconds(6);
        yield return null;

        // Find the story button
        GameObject storyButton = GameObject.Find("StoryButton");
        Assert.IsNotNull(storyButton, "Story button not found in the scene.");

        // Simulate a button press
        Button storyButtonComponent = storyButton.GetComponent<Button>();
        Assert.IsNotNull(storyButtonComponent, "Button component not found on the Story button.");
        storyButtonComponent.onClick.Invoke();

        // Allow some time for the scene transition
        yield return new WaitForSeconds(0.6f); // Adjust this duration as necessary

        // Check if the Story scene is loaded
        Scene activeScene = SceneManager.GetActiveScene();
        Assert.AreEqual(StorySceneName, activeScene.name, "The Level1 scene was not loaded after pressing the start button.");

        // Find next button
        GameObject nextButton = GameObject.Find("NextPageButton");
        Assert.IsNotNull(nextButton, "Next Page button not found in the scene.");

        // Simulate a button press
        Button nextButtonComponent = nextButton.GetComponent<Button>();
        Assert.IsNotNull(nextButtonComponent, "Button component not found on the Next Page button.");
        for (int i = 0; i < 4; i++)
        {
            nextButtonComponent.onClick.Invoke();
            yield return new WaitForSeconds(0.6f);
        }
    }

    [UnityTest]
    public IEnumerator TestForControlsButton()
    {
        // Load the menu scene
        yield return SceneManager.LoadSceneAsync(MenuSceneName);

        // Allow one frame for the scene to initialize
        yield return new WaitForSeconds(6);
        yield return null;

        // Find the controls button
        GameObject controlsButton = GameObject.Find("ControlsButton");
        Assert.IsNotNull(controlsButton, "Controls button not found in the scene.");

        // Simulate a button press
        Button controlsButtonComponent = controlsButton.GetComponent<Button>();
        Assert.IsNotNull(controlsButtonComponent, "Button component not found on the Controls button.");
        controlsButtonComponent.onClick.Invoke();

        // Allow some time for the scene transition
        yield return new WaitForSeconds(0.6f);

        // Check if the Story scene is loaded
        Scene activeScene = SceneManager.GetActiveScene();
        Assert.AreEqual(ControlsSceneName, activeScene.name, "The Controls scene was not loaded after pressing the controls button.");

        // Find Main Menu button
        GameObject menuButton = GameObject.Find("MainMenuButton");
        Assert.IsNotNull(menuButton, "Main Menu button not found in the Controls scene.");

        // Simulate a button press
        Button menuButtonComponent = menuButton.GetComponent<Button>();
        Assert.IsNotNull(menuButtonComponent, "Button component not found on the Main Menu button.");
        menuButtonComponent.onClick.Invoke();

        // Allow some time for the scene transition
        yield return new WaitForSeconds(0.6f);

        // Check if game is back in the main menu
        activeScene = SceneManager.GetActiveScene();
        Assert.AreEqual(MenuSceneName, activeScene.name, "The Menu scene was not loaded from the Controls scene.");
    }

    [UnityTest]
    public IEnumerator TestPlayerDeath()
    {
        // Load the menu scene
        yield return SceneManager.LoadSceneAsync(MenuSceneName);

        // Allow time for the scene to initialize
        yield return new WaitForSeconds(6);

        // Find the start button
        GameObject startButton = GameObject.Find("StartButton"); // Replace with your actual button name

        // Simulate a button press
        Button buttonComponent = startButton.GetComponent<Button>();
        buttonComponent.onClick.Invoke();

        // Allow some time for the scene transition
        yield return new WaitForSeconds(1); // Adjust this duration as necessary

        // Find the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        // player.GetComponent<Health>().TakeDamage(3);

        // // Wait for a moment to allow the game over screen to potentially appear
        // yield return new WaitForSeconds(1);

        // // Find the game over screen
        // GameObject deathScreen = GameObject.Find("GameOverScreen");

        // // Assert that the game over screen is active
        // Assert.IsTrue(deathScreen.activeSelf, "Game over screen should be active after player death.");
    }
}
